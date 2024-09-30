using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Category { get; set; }

        public int id { get; set; }
    }
}
