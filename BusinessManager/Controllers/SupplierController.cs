using BusinessManager.Models;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class SupplierController : Controller
    {
        private readonly BdEfcoreContext _context; 

        public SupplierController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var suppliers = await _context.Suppliers
                    .OrderBy(s => s.SupplierId)
                    .ToListAsync();

                return View(suppliers);
            }
            catch (Exception ex)
            {
                return View(new List<Supplier>());
            } 
        }

        // Create: GET 
        [HttpGet]
        public IActionResult CreateSupplierModal()
        {
            ViewBag.ModalTitle = "Crear Nuevo Proveedor";
            ViewBag.ActionName = "CreateSupplier";

            return PartialView("_SupplierFormPartial", new SupplierViewModel());
        }

        // Edit: GET 
        [HttpGet]
        public async Task<IActionResult> EditSupplierModal(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound(); 
            }

            var viewModel = new SupplierViewModel
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                Phone = supplier.Phone,
                Address = supplier.Address,
            };

            ViewBag.ModalTitle = "Editar Proveedor";
            ViewBag.ActionName = "EditSupplier";

            return PartialView("_SupplierFormPartial", viewModel);
        }

        // Create: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSupplier(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supplier = new Supplier
                    {
                        Name = model.Name,
                        ContactName = model.ContactName,
                        Phone = model.Phone,
                        Address = model.Address,
                    };

                    _context.Add(supplier);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Proveedor creado correctamente." });
                }
                catch (Exception ex) 
                {
                    return Json(new { success = false, message = "Error al crear el proveedor." });
                }
            }

            ViewBag.ModalTitle = "Crear Proveedor";
            ViewBag.ActionName = "CreateSupplier";

            return PartialView("_SupplierFormPartial", model);
        }

        // Edit: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupplier(int id, SupplierViewModel model)
        {
            if (id != model.SupplierId)
            {
                return NotFound();  
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSupplier = await _context.Suppliers.FindAsync(model.SupplierId);

                    if (existingSupplier == null)
                    {
                        return Json(new { success = false, message = "Proveedor no encontrado para la edición." });
                    }

                    existingSupplier.Name = model.Name;
                    existingSupplier.ContactName = model.ContactName;
                    existingSupplier.Phone = model.Phone;
                    existingSupplier.Address = model.Address;

                    _context.Update(existingSupplier);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Proveedor actualizado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar el proveedor." });
                }
            }

            ViewBag.ModalTitle = "Editar Proveedor";
            ViewBag.ActionName = "EditSupplier";

            return PartialView("_SupplierFormPartial", model);
        }
    }
}
