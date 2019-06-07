namespace LvivDotNet.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}
