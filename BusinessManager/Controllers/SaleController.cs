using BusinessManager.Models;
using BusinessManager.Models.Data;
using BusinessManager.Models.Enums;
using BusinessManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class SaleController : Controller
    {
        private readonly BdEfcoreContext _context;

        public SaleController(BdEfcoreContext context)
        {
            _context = context;
        }

        // Index: Historial de ventas
        public async Task<IActionResult> Index()
        {
            try
            {
                var sales = await _context.Sales
                    .OrderByDescending(s => s.SaleDate)
                    .Include(s => s.Customer)
                    .Include(s => s.User)
                    .Include(s => s.SaleDetails)
                        .ThenInclude(sd => sd.Product)
                    .ToListAsync();

                return View(sales);
            }
            catch (Exception ex)
            {
                return View(new List<Sale>());
            }
        }

        // Create: GET - Pantalla POS
        // Pantalla POS - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new SaleViewModel
            {
                SaleDate = DateTime.Now,
                Details = new List<SaleDetailViewModel>()
            };

            // Cargar datos para los dropdowns
            ViewData["Customers"] = new SelectList(
                await _context.Customers.OrderBy(c => c.Name).ToListAsync(),
                "CustomerId",
                "Name"
            );

            // Cargar productos con su precio de venta y stock
            var allProducts = await _context.Products
                .Include(p => p.InventoryMovements)
                .ToListAsync(); // ✅ Primero traer a memoria

            // ✅ Filtrar en memoria (no en SQL)
            var products = allProducts
                .Select(p => new {
                    p.ProductId,
                    p.Name,
                    p.SalePrice,
                    Stock = p.CurrentStock // Ahora sí puede usar CurrentStock
                })
                .Where(p => p.Stock > 0) // Filtrar después de materializar
                .ToList();

            ViewData["Products"] = new SelectList(products, "ProductId", "Name");
            ViewData["ProductPrices"] = products.ToDictionary(p => p.ProductId, p => p.SalePrice);
            ViewData["ProductStock"] = products.ToDictionary(p => p.ProductId, p => p.Stock);

            return View(viewModel);
        }

        // Create: POST - Guardar venta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar stock disponible
                    foreach (var detail in model.Details)
                    {
                        var product = await _context.Products
                            .Include(p => p.InventoryMovements)
                            .FirstOrDefaultAsync(p => p.ProductId == detail.ProductId);

                        if (product == null)
                        {
                            ModelState.AddModelError("", $"Producto no encontrado.");
                            return await ReloadCreateView(model);
                        }

                        if (product.CurrentStock < detail.Quantity)
                        {
                            ModelState.AddModelError("", $"Stock insuficiente para {product.Name}. Disponible: {product.CurrentStock}");
                            return await ReloadCreateView(model);
                        }
                    }

                    // Crear la venta
                    var sale = new Sale
                    {
                        CustomerId = model.CustomerId,
                        UserId = 1, // TODO: Obtener del usuario autenticado
                        SaleDate = model.SaleDate,
                        TotalAmount = model.Details.Sum(d => d.SubTotal),
                        Notes = model.Notes,
                        CreatedAt = DateTime.Now,
                        SaleDetails = model.Details.Select(d => new SaleDetail
                        {
                            ProductId = d.ProductId,
                            Quantity = d.Quantity,
                            UnitPrice = d.UnitPrice,
                            SubTotal = d.SubTotal
                        }).ToList()
                    };

                    _context.Add(sale);
                    await _context.SaveChangesAsync();

                    // Crear movimientos de inventario (salida de stock)
                    foreach (var detail in model.Details)
                    {
                        var movement = new InventoryMovement
                        {
                            ProductId = detail.ProductId,
                            MovementType = MovementTypeEnum.SALIDA,
                            Quantity = detail.Quantity,
                            Notes = $"Venta #{sale.SaleId}",
                            CreatedAt = DateTime.Now
                        };
                        _context.InventoryMovements.Add(movement);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Venta registrada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al registrar la venta.");
                }
            }

            return await ReloadCreateView(model);
        }

        // Método auxiliar para recargar la vista Create con datos
        // Método auxiliar para recargar la vista Create con datos
        private async Task<IActionResult> ReloadCreateView(SaleViewModel model)
        {
            ViewData["Customers"] = new SelectList(
                await _context.Customers.OrderBy(c => c.Name).ToListAsync(),
                "CustomerId",
                "Name",
                model.CustomerId
            );

            // ✅ Mismo cambio aquí
            var allProducts = await _context.Products
                .Include(p => p.InventoryMovements)
                .ToListAsync();

            var products = allProducts
                .Select(p => new {
                    p.ProductId,
                    p.Name,
                    p.SalePrice,
                    Stock = p.CurrentStock
                })
                .Where(p => p.Stock > 0)
                .ToList();

            ViewData["Products"] = new SelectList(products, "ProductId", "Name");
            ViewData["ProductPrices"] = products.ToDictionary(p => p.ProductId, p => p.SalePrice);
            ViewData["ProductStock"] = products.ToDictionary(p => p.ProductId, p => p.Stock);

            return View(model);
        }
    }
}