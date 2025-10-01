using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [StringLength(100, ErrorMessage = "El nombre de contacto no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre de Contacto")]
        public string? ContactName { get; set; }

        [StringLength(25, ErrorMessage = "El teléfono no puede exceder los 25 caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "La dirección no puede exceder los 100 caracteres.")]
        [Display(Name = "Dirección")]
        public string? Address { get; set; }
    }
}
