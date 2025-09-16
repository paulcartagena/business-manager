using BusinessManager.Models;
using BusinessManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BusinessManager.Controllers
{
    public class RolController : Controller
    {
        private readonly BdEfcoreContext _context; 
        public RolController(BdEfcoreContext context)
        {
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var rols = await _context.Rols
                .OrderBy(r => r.RolId)
                .ToListAsync();

                return View(rols);
            }
            catch (Exception ex)
            {
                return View(new List<Rol>());
            }
        }

        public IActionResult CreateRolModal() 
        {
            ViewBag.ModalTitle = "Crear Nuevo Rol";
            ViewBag.ActionName = "CreateRol";

            return PartialView("_RolFormPartial", new RolViewModel());
        }

        public async Task<IActionResult> EditRolModal(int id)
        {
            var rol = await _context.Rols.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            var viewModel = new RolViewModel
            {
                RolId = rol.RolId,
                Name = rol.Name,
            };

            ViewBag.ModalTitle = "Editar Rol";
            ViewBag.ActionName = "EditRol";

            return PartialView("_RolFormPartial", viewModel); 
        }

        public async Task<IActionResult> CreateRol(RolViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rol = new Rol()
                    {
                        Name = model.Name,
                    };

                    _context.Rols.Add(rol);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Rol creado correctamente." });
                }
                catch (Exception ex) 
                {
                    return Json(new { success = false, message = "Error al crear el rol." });
                }
            }
            ViewBag.ModalTitle = "Editar Rol";
            ViewBag.ActionName = "EditRol";

            return PartialView("_RolFormPartial", model);
        }

    }
}
