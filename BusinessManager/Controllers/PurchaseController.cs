using BusinessManager.Models;
using BusinessManager.Models.Data;
using BusinessManager.Models.Enums;
using BusinessManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly BdEfcoreContext _context;

        public PurchaseController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var purchases = await _context.Purchases
                    .OrderByDescending(p => p.PurchaseDate)
                    .Include(p => p.Supplier)
                    .Include(p => p.User)
                    .Include(p => p.PurchaseDetails)
                        .ThenInclude(pd => pd.Product)
                    .ToListAsync();

                return View(purchases);
            }
            catch (Exception ex)
            {
                return View(new List<Purchase>());
            }
        }

        // Create: GET
        [HttpGet]
        public IActionResult CreatePurchaseModal()
        {
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "Name");

            // ✅ CAMBIO: Usar precio promedio de compra
            var products = _context.Products
                .Include(p => p.PurchaseDetails)
                .Select(p => new {
                    p.ProductId,
                    p.Name,
                    AveragePrice = p.PurchaseDetails.Any()
                        ? p.PurchaseDetails.Average(pd => pd.UnitPrice)
                        : 0
                })
                .ToList();

            ViewData["Products"] = new SelectList(products, "ProductId", "Name");
            ViewData["ProductPrices"] = products.ToDictionary(p => p.ProductId, p => p.AveragePrice);

            ViewBag.ModalTitle = "Crear Nueva Compra";
            ViewBag.ActionName = "CreatePurchase";

            return PartialView("_PurchaseFormPartial", new PurchaseViewModel
            {
                PurchaseDate = DateTime.Now,
                Details = new List<PurchaseDetailViewModel>()
            });
        }

        // Edit: GET
        [HttpGet]
        public async Task<IActionResult> EditPurchaseModal(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseDetails)
                .FirstOrDefaultAsync(p => p.PurchaseId == id);

            if (purchase == null) return NotFound();

            var viewModel = new PurchaseViewModel
            {
                PurchaseId = purchase.PurchaseId,
                SupplierId = purchase.SupplierId,
                UserId = purchase.UserId,
                PurchaseDate = purchase.PurchaseDate,
                TotalAmount = purchase.TotalAmount,
                Notes = purchase.Notes,
                Details = purchase.PurchaseDetails.Select(pd => new PurchaseDetailViewModel
                {
                    PurchaseDetailId = pd.PurchaseDetailId,
                    ProductId = pd.ProductId,
                    Quantity = pd.Quantity,
                    UnitPrice = pd.UnitPrice
                }).ToList()
            };

            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "Name", viewModel.SupplierId);

            // ✅ CAMBIO: Usar precio promedio de compra
            var products = _context.Products
                .Include(p => p.PurchaseDetails)
                .Select(p => new {
                    p.ProductId,
                    p.Name,
                    AveragePrice = p.PurchaseDetails.Any()
                        ? p.PurchaseDetails.Average(pd => pd.UnitPrice)
                        : 0
                })
                .ToList();

            ViewData["Products"] = new SelectList(products, "ProductId", "Name");
            ViewData["ProductPrices"] = products.ToDictionary(p => p.ProductId, p => p.AveragePrice);

            ViewBag.ModalTitle = "Editar Compra";
            ViewBag.ActionName = "EditPurchase";

            return PartialView("_PurchaseFormPartial", viewModel);
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchase(PurchaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var purchase = new Purchase
                    {
                        SupplierId = model.SupplierId,
                        UserId = 1, // TODO: Obtener del usuario autenticado
                        PurchaseDate = model.PurchaseDate,
                        TotalAmount = model.Details.Sum(d => d.SubTotal),
                        Notes = model.Notes,
                        CreatedAt = DateTime.Now,
                        PurchaseDetails = model.Details.Select(d => new PurchaseDetail
                        {
                            ProductId = d.ProductId,
                            Quantity = d.Quantity,
                            UnitPrice = d.UnitPrice,
                            SubTotal = d.SubTotal
                        }).ToList()
                    };

                    _context.Add(purchase);
                    await _context.SaveChangesAsync();

                    // ✅ NUEVO: Crear movimientos de inventario (entrada de stock)
                    foreach (var detail in model.Details)
                    {
                        var movement = new InventoryMovement
                        {
                            ProductId = detail.ProductId,
                            MovementType = MovementTypeEnum.ENTRADA,
                            Quantity = detail.Quantity,
                            Notes = $"Compra #{purchase.PurchaseId}",
                            CreatedAt = DateTime.Now
                        };
                        _context.InventoryMovements.Add(movement);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Compra creada correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear la compra." });
                }
            }

            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "Name", model.SupplierId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductId", "Name");

            ViewBag.ModalTitle = "Crear Nueva Compra";
            ViewBag.ActionName = "CreatePurchase";

            return PartialView("_PurchaseFormPartial", model);
        }

        // Edit: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPurchase(int id, PurchaseViewModel model)
        {
            if (id != model.PurchaseId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPurchase = await _context.Purchases
                        .Include(p => p.PurchaseDetails)
                        .FirstOrDefaultAsync(p => p.PurchaseId == model.PurchaseId);

                    if (existingPurchase == null)
                    {
                        return Json(new { success = false, message = "Compra no encontrada." });
                    }

                    // ✅ NUEVO: Revertir movimientos de inventario anteriores
                    var oldMovements = await _context.InventoryMovements
                        .Where(m => m.Notes == $"Compra #{existingPurchase.PurchaseId}")
                        .ToListAsync();
                    _context.InventoryMovements.RemoveRange(oldMovements);

                    // Actualizar cabecera
                    existingPurchase.SupplierId = model.SupplierId;
                    existingPurchase.PurchaseDate = model.PurchaseDate;
                    existingPurchase.TotalAmount = model.Details.Sum(d => d.SubTotal);
                    existingPurchase.Notes = model.Notes;

                    // Eliminar detalles anteriores
                    existingPurchase.PurchaseDetails.Clear();

                    // Agregar nuevos detalles
                    existingPurchase.PurchaseDetails = model.Details.Select(d => new PurchaseDetail
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        SubTotal = d.SubTotal
                    }).ToList();

                    _context.Update(existingPurchase);
                    await _context.SaveChangesAsync();

                    // ✅ NUEVO: Crear nuevos movimientos de inventario
                    foreach (var detail in model.Details)
                    {
                        var movement = new InventoryMovement
                        {
                            ProductId = detail.ProductId,
                            MovementType = MovementTypeEnum.ENTRADA,
                            Quantity = detail.Quantity,
                            Notes = $"Compra #{existingPurchase.PurchaseId}",
                            CreatedAt = DateTime.Now
                        };
                        _context.InventoryMovements.Add(movement);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Compra actualizada correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar la compra." });
                }
            }

            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "Name", model.SupplierId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductId", "Name");

            ViewBag.ModalTitle = "Editar Compra";
            ViewBag.ActionName = "EditPurchase";

            return PartialView("_PurchaseFormPartial", model);
        }
    }
}