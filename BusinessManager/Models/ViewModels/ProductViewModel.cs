using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels; 

public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")] 
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!; 

    [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; } = null!;

    [Required(ErrorMessage = "El precio de compra es requerido.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    [Display(Name = "Precio de Compra")]
    public decimal PurchasePrice { get; set; }

    [Required(ErrorMessage = "El precio de venta es requerido.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    [Display(Name = "Precio de Venta")]
    public decimal SalePrice { get; set; }

    [Required(ErrorMessage = "La categoría es requerida.")]
    [Display(Name = "Categoría")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "La unidad de medida es requerida.")]
    [Display(Name = "Udm")]
    public int UomId { get; set; }
}