using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models
{
    [Table("supplier")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("contact_name")]
        [StringLength(100)]
        public string? ContactName { get; set; }

        [Column("phone")]
        [StringLength(25)]
        public string? Phone {  get; set; }

        [Column("address")]
        [StringLength(100)]
        public string? Address { get; set; }

        [InverseProperty("Supplier")]
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
