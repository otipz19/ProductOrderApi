using WebApi.Models.ProductInOrder;

namespace WebApi.Models.Order
{
    public class UpsertOrderDto
    {
        public List<ProductInOrderDto> ProductsInOrder { get; set; }
    }
}
