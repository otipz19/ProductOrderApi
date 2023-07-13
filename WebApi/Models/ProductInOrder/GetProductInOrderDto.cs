using WebApi.Models.Product;

namespace WebApi.Models.ProductInOrder
{
    public class GetProductInOrderDto
    {
        public int Amount { get; set; }

        public GetProductDto Product { get; set; }
    }
}
