using BusinessManager;
using BusinessManager.Models;
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

        // Create
        [HttpGet]
        public IActionResult CreateUomModal()
        {
            ViewBag.ModalTitle = "Crear Nueva U. de Medida";
            ViewBag.ActionName = "CreateUom"; // Acción POST 

            // Devuelve la vista parcial con un nuevo ViewModel vacío
            return PartialView("_UomFormPartial", new UomViewModel { IsActive = true });
        }

        // Edit 
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
                IsWeightUnit = uom.IsWeightUnit,
                IsActive = uom.IsActive,
            };

            ViewBag.ModalTitle = "Editar Uom";
            ViewBag.ActionName = "EditUom"; 

            return PartialView("_UomFormPartial", viewModel);
        }

        // Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUom(UomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uom = new Uom()
                {
                    Name = model.Name,
                    IsWeightUnit = model.IsWeightUnit,
                    IsActive = model.IsActive,
                };

                _context.Add(uom);
                await _context.SaveChangesAsync();

                // --- CAMBIO: Devolver JSON si es exitoso ---
                return Json(new { success = true, message = "U. de Medida creada correctamente." });
            }

            // --- CAMBIO: Si hay errores ---
            ViewBag.ModalTitle = "Crear Nueva U. de Medida";
            ViewBag.ActionName = "CreateUom";
            return PartialView("_UomFormPartial", model); 
        }

        // Edit 
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
                    existingUom.IsActive = model.IsActive;

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
                        throw;
                    }
                }
            }

            ViewBag.ModalTitle = "Editar Uom";
            ViewBag.ActionName = "EditUom";

            return PartialView("_UomFormPartial", model);
        }
    }
}