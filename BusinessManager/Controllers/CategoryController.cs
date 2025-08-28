using BusinessManager;
using BusinessManager.Models;
using BusinessManager.Models.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BdEfcoreContext _context;
        public CategoryController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(u => u.Name)
                .ToListAsync();
            return View(categories);
        }

        // Create
        [HttpGet]
        public IActionResult CreateCategoryModal()
        {
            ViewBag.ModalTitle = "Crear Nueva Categoría";
            ViewBag.ActionName = "CreateCategory"; // Acción POST 

            // Devuelve la vista parcial con un nuevo ViewModel vacío
            return PartialView("_CategoryFormPartial", new CategoryViewModel { IsActive = true });
        }

        // Edit
        [HttpGet]
        public async Task<IActionResult> EditCategoryModal(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                CategoryId = id,
                Name = category.Name,
                IsActive = category.IsActive
            };

            ViewBag.ModalTitle = "Editar Categoría";
            ViewBag.ActionName = "EditCategory";

            return PartialView("_CategoryFormPartial", viewModel);
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name, 
                    IsActive = model.IsActive
                };

                _context.Add(category);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Categoría creada correctamente." });
            }

            // --- CAMBIO: Si hay errores ---
            ViewBag.ModalTitle = "Crear Nueva Categoría";
            ViewBag.ActionName = "CreateCategory";
            return PartialView("_CategoryFormPartial", model); // Devuelve la vista con los errores
        }

        // Edit: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, CategoryViewModel model)
        {
            if (id != model.CategoryId) 
            { 
                return NotFound();
            }

            if (ModelState.IsValid) 
            { 
                try
                {
                    var existingCategory = await _context.Categories.FindAsync(model.CategoryId);

                    if (existingCategory == null) 
                    {
                        return Json(new { success = false, message = "Categoría no encontrada para la edición." });
                    }

                    existingCategory.Name = model.Name;
                    existingCategory.IsActive = model.IsActive;

                    _context.Update(existingCategory);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Categoría actualizada correctamente." });
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!await _context.Categories.AnyAsync(e => e.CategoryId == model.CategoryId))
                    {
                        return Json(new { success = false, message = "La categoría ya no existe." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.ModalTitle = "Editar Categoría";
            ViewBag.ActionName = "EditCategory";

            return PartialView("_CategoryFormPartial", model);
        }
    }
}