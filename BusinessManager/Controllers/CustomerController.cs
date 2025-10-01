using BusinessManager.Models;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BdEfcoreContext _context; 

        public CustomerController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _context.Customers
                    .OrderBy(c => c.CustomerId)
                    .ToListAsync();

                return View(customers);
            }
            catch (Exception ex)
            {
                return View(new List<Customer>());
            }
        }

        // Create: GET 
        [HttpGet]
        public IActionResult CreateCustomerModal()
        {
            ViewBag.ModalTitle = "Crear Nuevo Cliente";
            ViewBag.ActionName = "CreateCustomer";

            return PartialView("_CustomerFormPartial", new CustomerViewModel()); 
        }

        // Edit: GET
        [HttpGet]
        public async Task<IActionResult> EditCustomerModal(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var viewModel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                ContactName = customer.ContactName,
                Phone = customer.Phone,
                Address = customer.Address,
            };

            ViewBag.ModalTitle = "Editar Cliente";
            ViewBag.ActionName = "EditCustomer";

            return PartialView("_CustomerFormPartial", viewModel);
        }

        // Create: POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = new Customer
                    {
                        Name = model.Name,
                        ContactName = model.ContactName,
                        Phone = model.Phone,
                        Address = model.Address,
                    };

                    _context.Add(customer);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Cliente creado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear el cliente." });
                }
            }

            ViewBag.ModalTitle = "Crear Cliente";
            ViewBag.ActionName = "CreateCustomer";

            return PartialView("_CustomerFormPartial", model);
        }

        // Edit: POST 
        [HttpPost]
        public async Task<IActionResult> EditCustomer(int id, CustomerViewModel model)
        {
            if (id != model.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers.FindAsync(model.CustomerId);

                    if (existingCustomer == null)
                    {
                        return Json(new { success = false, message = "Cliente no encontrado para la edición." });
                    }

                    existingCustomer.Name = model.Name;
                    existingCustomer.ContactName = model.ContactName;
                    existingCustomer.Phone = model.Phone;
                    existingCustomer.Address = model.Address;

                    _context.Update(existingCustomer);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Cliente actualizado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar el cliente." });
                }
            }

            ViewBag.ModalTitle = "Editar Cliente";
            ViewBag.ActionName = "EditCustomer";

            return PartialView("_CustomerFormPartial", model);
        }
    }
}
