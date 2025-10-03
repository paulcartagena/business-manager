using BusinessManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models.ViewModels
{
    public class InventoryMovementViewModel
    {
        public int InventoryMovementId { get; set; }

        [Required(ErrorMessage = "El producto es requerido")]
        [Display(Name = "Producto")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [Display(Name = "Tipo de Movimiento")]
        public MovementTypeEnum MovementType { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }

        [StringLength(250)]
        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ProductName { get; set; }
    }
}