using BusinessManager;
using BusinessManager.Models;
using BusinessManager.Models.Data;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class UomController : Controller
    {
        private readonly BdEfcoreContext _context;
        public UomController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var uoms = await _context.Uoms
                .OrderBy(u => u.Name)
                .ToListAsync();
            return View(uoms);
        }

        // Create: GET
        [HttpGet]
        public IActionResult CreateUomModal()
        {
            ViewBag.ModalTitle = "Crear Nueva U. de Medida";
            ViewBag.ActionName = "CreateUom";

            return PartialView("_UomFormPartial", new UomViewModel());
        }

        // Edit: GET
        [HttpGet]
        public async Task<IActionResult> EditUomModal(int id)
        {
            var uom = await _context.Uoms.FindAsync(id);
            if (uom == null)
            {
                return NotFound();
            }

            var viewModel = new UomViewModel
            {
                UomId = uom.UomId,
                Name = uom.Name,
                IsWeightUnit = uom.IsWeightUnit
            };

            ViewBag.ModalTitle = "Editar Uom";
            ViewBag.ActionName = "EditUom";

            return PartialView("_UomFormPartial", viewModel);
        }

        // Create: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUom(UomViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uom = new Uom()
                    {
                        Name = model.Name,
                        IsWeightUnit = model.IsWeightUnit
                    };

                    _context.Add(uom);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "U. de Medida creada correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear la unidad de medida: " + ex.Message });
                }
            }

            ViewBag.ModalTitle = "Crear Nueva U. de Medida";
            ViewBag.ActionName = "CreateUom";
            return PartialView("_UomFormPartial", model);
        }

        // Edit: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUom(int id, UomViewModel model)
        {
            if (id != model.UomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUom = await _context.Uoms.FindAsync(model.UomId);
                    if (existingUom == null)
                    {
                        return Json(new { success = false, message = "Unidad de medida no encontrada para la edición." });
                    }

                    existingUom.Name = model.Name;
                    existingUom.IsWeightUnit = model.IsWeightUnit;

                    _context.Update(existingUom);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Unidad de medida actualizada correctamente." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Uoms.AnyAsync(e => e.UomId == model.UomId))
                    {
                        return Json(new { success = false, message = "La unidad de medida ya no existe." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error de concurrencia. La unidad de medida fue modificada por otro usuario." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar la unidad de medida: " + ex.Message });
                }
            }

            ViewBag.ModalTitle = "Editar Uom";
            ViewBag.ActionName = "EditUom";

            return PartialView("_UomFormPartial", model);
        }
    }
}