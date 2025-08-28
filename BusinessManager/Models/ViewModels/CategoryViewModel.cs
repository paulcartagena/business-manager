using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels; 
public class CategoryViewModel
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;
    public Boolean IsActive { get; set; }
}
