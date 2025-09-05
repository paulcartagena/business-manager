using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels
{
    public class UomViewModel
    {
        public int UomId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de unidad es requerido.")]
        [Display(Name = "Tipo de Unidad")]
        public Boolean IsWeightUnit { get; set; }
    }
}
