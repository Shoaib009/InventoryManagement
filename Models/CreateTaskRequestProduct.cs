namespace InventoryManagement.Models
{
    public class CreateTaskRequestProduct
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; } 
    }
}
