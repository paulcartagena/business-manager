using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusinessManager.ViewModels
{
    public class SaleViewModel
    {
        public int SaleId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "Fecha de Venta")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Display(Name = "Total")]
        public decimal TotalAmount { get; set; }

        [StringLength(100)]
        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<SaleDetailViewModel> Details { get; set; } = new();
    }

    public class SaleDetailViewModel
    {
        public int SaleDetailId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }

        public decimal SubTotal => Quantity * UnitPrice;
    }
}