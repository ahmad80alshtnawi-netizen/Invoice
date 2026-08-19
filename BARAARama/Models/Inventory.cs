namespace BARAARama.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int MaterialId { get; set; }

        public Material Material { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}