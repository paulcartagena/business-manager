using BusinessManager.Models;
using BusinessManager.Models.Enums;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class InventoryMovementController : Controller
    {
        private readonly BdEfcoreContext _context;

        public InventoryMovementController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var movements = await _context.InventoryMovements
                    .Include(m => m.Product)
                    .OrderByDescending(m => m.CreatedDate)
                    .ToListAsync();

                return View(movements);
            }
            catch (Exception ex)
            {
                return View(new List<InventoryMovement>());
            }
        }

        // Create: GET
        [HttpGet]
        public async Task<IActionResult> CreateMovementModal()
        {
            ViewData["Products"] = new SelectList(
                await _context.Products.OrderBy(p => p.Name).ToListAsync(),
                "ProductId",
                "Name"
            );

            ViewData["MovementTypes"] = new SelectList(
                Enum.GetValues(typeof(MovementTypeEnum)).Cast<MovementTypeEnum>()
            );

            ViewBag.ModalTitle = "Crear Movimiento de Inventario";
            ViewBag.ActionName = "CreateMovement";

            return PartialView("_InventoryMovementFormPartial", new InventoryMovementViewModel
            {
                Quantity = 1
            });
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovement(InventoryMovementViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Validar que el producto existe
                    var product = await _context.Products
                        .Include(p => p.InventoryMovements)
                        .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

                    if (product == null)
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }

                    // 2. Para salidas, validar stock disponible
                    if (model.MovementType == MovementTypeEnum.SALIDA)
                    {
                        int currentStock = product.CurrentStock;
                        if (currentStock < model.Quantity)
                        {
                            return Json(new { success = false, message = $"Stock insuficiente. Disponible: {currentStock}, Solicitado: {model.Quantity}" });
                        }
                    }

                    // 3. Crear el movimiento
                    var movement = new InventoryMovement
                    {
                        ProductId = model.ProductId,
                        MovementType = model.MovementType,
                        Quantity = model.Quantity,
                        Notes = model.Notes,
                        CreatedDate = DateTime.Now
                    };

                    _context.InventoryMovements.Add(movement);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Movimiento creado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear el movimiento: " + ex.Message });
                }
            }

            ViewData["Products"] = new SelectList(
                await _context.Products.OrderBy(p => p.Name).ToListAsync(),
                "ProductId",
                "Name",
                model.ProductId
            );

            ViewData["MovementTypes"] = new SelectList(
                Enum.GetValues(typeof(MovementTypeEnum)).Cast<MovementTypeEnum>(),
                model.MovementType
            );

            ViewBag.ModalTitle = "Crear Movimiento de Inventario";
            ViewBag.ActionName = "CreateMovement";

            return PartialView("_InventoryMovementFormPartial", model);
        }

        // Ver stock actual de un producto (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetProductStock(int productId)
        {
            var product = await _context.Products
                .Include(p => p.InventoryMovements)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            return Json(new
            {
                success = true,
                stock = product.CurrentStock,
                productName = product.Name,
                purchasePrice = product.PurchasePrice
            });
        }
    }
}