using BusinessManager.Models;
using BusinessManager.Models.Data;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Controllers
{
    public class UserController : Controller
    {
        private readonly BdEfcoreContext _context;
        public UserController(BdEfcoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderBy(u => u.Name)
                .Include(u => u.Rol)
                .ToListAsync();
            return View(users);
        }

        // Create: GET 
        [HttpGet]
        public IActionResult CreateUserModal()
        {
            ViewBag.ModalTitle = "Crear Nuevo Usuario";
            ViewBag.ActionName = "CreateUser";

            ViewData["Rols"] = new SelectList(_context.Rols, "RolId", "Name");

            return PartialView("_UserFormPartial", new UserViewModel());
        }

        // Edit: GET 
        [HttpGet]
        public async Task<IActionResult> EditUserModal(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Mail = user.Mail,
                RolId = user.RolId
            };

            ViewData["Rols"] = new SelectList(_context.Rols, "RolId", "Name");

            ViewBag.ModalTitle = "Editar Usuario";
            ViewBag.ActionName = "EditUser";

            return PartialView("_UserFormPartial", viewModel);
        }

        // Create: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User()
                    {
                        Name = model.Name,
                        Mail = model.Mail,
                        RolId = model.RolId
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Usuario creado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear el usuario: " + ex.Message });
                }
            }

            ViewBag.ModalTitle = "Crear Nuevo Usuario";
            ViewBag.ActionName = "CreateUser";
            ViewData["Rols"] = new SelectList(_context.Rols, "RolId", "Name", model.RolId);

            return PartialView("_UserFormPartial", model);
        }

        // Edit: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, UserViewModel model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(model.UserId);
                    if (existingUser == null)
                    {
                        return Json(new { success = false, message = "Usuario no encontrado para la edición." });
                    }

                    existingUser.Name = model.Name;
                    existingUser.Mail = model.Mail;
                    existingUser.RolId = model.RolId;

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Usuario actualizado correctamente." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Users.AnyAsync(e => e.UserId == model.UserId))
                    {
                        return Json(new { success = false, message = "El usuario ya no existe." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error de concurrencia. El usuario fue modificado por otro usuario." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar el usuario: " + ex.Message });
                }
            }

            ViewData["Rols"] = new SelectList(_context.Rols, "RolId", "Name", model.RolId);

            ViewBag.ModalTitle = "Editar Usuario";
            ViewBag.ActionName = "EditUser";

            return PartialView("_UserFormPartial", model);
        }
    }
}