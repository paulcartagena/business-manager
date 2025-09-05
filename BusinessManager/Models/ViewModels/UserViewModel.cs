using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El mail es requerido.")]
        public string Mail { get; set; } = null!;

        [Required(ErrorMessage = "El rol es requerido.")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }
    }   
}
