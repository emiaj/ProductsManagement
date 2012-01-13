namespace ProductsManagement.Domain.Entities
{
    public class Product
    {
        public Product()
        {
            
        }
        public Product(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}