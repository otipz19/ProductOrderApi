using WebApi.Models.ProductInOrder;

namespace WebApi.Models.Order
{
    public class UpsertOrderDto
    {
        public List<UpsertProductInOrderDto> ProductsInOrder { get; set; }
    }
}
