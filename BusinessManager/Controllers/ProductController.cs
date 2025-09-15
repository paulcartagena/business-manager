using BusinessManager.Models;
using BusinessManager.Models.Data;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class ProductController : Controller
    {
        private readonly BdEfcoreContext _context;

        public ProductController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _context.Products
                    .OrderBy(p => p.ProductId)
                    .Include(p => p.Category)
                    .Include(p => p.Uom)
                    .ToListAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                return View(new List<Product>());
            }
        }

        // Create: GET 
        [HttpGet]
        public IActionResult CreateProductModal()
        {
            ViewData["Categories"] = new SelectList(
                 _context.Categories,
                 "CategoryId",
                 "Name"
            );

            ViewData["Uoms"] = new SelectList(
                _context.Uoms,
                "UomId",
                "Name"
            );

            ViewBag.ModalTitle = "Crear Nuevo Producto";
            ViewBag.ActionName = "CreateProduct";

            return PartialView("_ProductFormPartial", new ProductViewModel
            {
                SalePrice = 0,
                PurchasePrice = 0,
                Stock = 0,
            });
        }

        // Edit: GET 
        [HttpGet]
        public async Task<IActionResult> EditProductModal(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var viewModel = new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                UomId = product.UomId,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                Stock = product.Stock
            };

            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", viewModel.CategoryId);
            ViewData["Uoms"] = new SelectList(_context.Uoms, "UomId", "Name", viewModel.UomId);

            ViewBag.ModalTitle = "Editar Producto";
            ViewBag.ActionName = "EditProduct";

            return PartialView("_ProductFormPartial", viewModel);
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        UomId = model.UomId,
                        PurchasePrice = model.PurchasePrice,
                        SalePrice = model.SalePrice,
                        Stock = model.Stock,
                    };

                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Producto creado correctamente." });
                }
                catch (Exception ex)
                {
                    // Si falla al crear el producto
                    return Json(new { success = false, message = "Error al crear el producto." });
                }
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
            ViewData["Uoms"] = new SelectList(_context.Uoms, "UomId", "Name", model.UomId);

            ViewBag.ModalTitle = "Crear Nuevo Producto";
            ViewBag.ActionName = "CreateProduct";

            return PartialView("_ProductFormPartial", model);
        }

        // Edit: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductViewModel model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(model.ProductId);
                    if (existingProduct == null)
                    {
                        return Json(new { success = false, message = "Producto no encontrado para la edición." });
                    }

                    existingProduct.Name = model.Name;
                    existingProduct.Description = model.Description;
                    existingProduct.CategoryId = model.CategoryId;
                    existingProduct.UomId = model.UomId;
                    existingProduct.PurchasePrice = model.PurchasePrice;
                    existingProduct.SalePrice = model.SalePrice;
                    existingProduct.Stock = model.Stock;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Producto actualizado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar el producto." });
                }
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
            ViewData["Uoms"] = new SelectList(_context.Uoms, "UomId", "Name", model.UomId);

            ViewBag.ModalTitle = "Editar Producto";
            ViewBag.ActionName = "EditProduct";

            return PartialView("_ProductFormPartial", model);
        }
    }
}