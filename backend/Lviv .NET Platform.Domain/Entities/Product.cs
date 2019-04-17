namespace Lviv_.NET_Platform.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
