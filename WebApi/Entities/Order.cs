using WebApi.Enums;

namespace WebApi.Entities
{
    public class Order : BaseEntity
    {
        public OrderStatus Status { get; set; }

        public DateTime StatusChangedAt { get; set; }

        public double OrderTotal { get; set; }

        public virtual List<ProductInOrder> ProductsInOrder { get; set; } = new();

        public virtual List<Product> Products { get; set; } = new();
    }
}
