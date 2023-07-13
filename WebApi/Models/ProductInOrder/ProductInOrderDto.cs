using WebApi.Models.Product;

namespace WebApi.Models.ProductInOrder
{
    public class ProductInOrderDto
    {
        public int Amount { get; set; }

        public GetProductDto Product { get; set; }
    }
}
