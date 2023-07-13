using WebApi.Enums;
using WebApi.Models.ProductInOrder;

namespace WebApi.Models.Order
{
    public class GetOrderDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime StatusChangedAt { get; set; }

        public double OrderTotal { get; set; }

        public List<ProductInOrderDto> Products { get; set; }
    }
}
