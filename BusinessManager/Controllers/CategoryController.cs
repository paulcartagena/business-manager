using BusinessManager;
using BusinessManager.Models;
using BusinessManager.Models.Data;
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
            try
            {
                var categories = await _context.Categories
               .OrderBy(u => u.CategoryId)
               .ToListAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                return View(new List<Category>());
            }
           
        }

        // Create: GET 
        [HttpGet]
        public IActionResult CreateCategoryModal()
        {
            ViewBag.ModalTitle = "Crear Nueva Categoría";
            ViewBag.ActionName = "CreateCategory";

            return PartialView("_CategoryFormPartial", new CategoryViewModel());
        }

        // Edit: POST 
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
                Name = category.Name
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
                try
                {
                    var category = new Category()
                    {
                        Name = model.Name
                    };

                    _context.Add(category);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Categoría creada correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear la categoría: " + ex.Message });
                }
            }

            ViewBag.ModalTitle = "Crear Nueva Categoría";
            ViewBag.ActionName = "CreateCategory";
            return PartialView("_CategoryFormPartial", model);
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

                    _context.Update(existingCategory);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Categoría actualizada correctamente." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Categories.AnyAsync(e => e.CategoryId == model.CategoryId))
                    {
                        return Json(new { success = false, message = "La categoría ya no existe." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error de concurrencia. La categoría fue modificada por otro usuario." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar la categoría: " + ex.Message });
                }
            }

            ViewBag.ModalTitle = "Editar Categoría";
            ViewBag.ActionName = "EditCategory";

            return PartialView("_CategoryFormPartial", model);
        }
    }
}