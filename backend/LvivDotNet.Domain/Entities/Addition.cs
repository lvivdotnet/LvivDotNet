namespace LvivDotNet.Domain.Entities
{
    public class Addition: BaseEntity
    {
        public byte[] Blob { get; set; }

        public string Title { get; set; }

        public int EventId { get; set; }

        public int PostId { get; set; }

        public int ProductId { get; set; }
    }
}
