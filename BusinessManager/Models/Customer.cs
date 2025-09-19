using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("contact_name")]
        [StringLength(100)]
        public string? ContactName { get; set; }

        [Column("phone")]
        [StringLength(25)]
        public string? Phone { get; set; }

        [Column("address")]
        [StringLength(100)]
        public string? Address { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
