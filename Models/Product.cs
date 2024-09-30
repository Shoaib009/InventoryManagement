using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [DefaultValue(1)]
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
