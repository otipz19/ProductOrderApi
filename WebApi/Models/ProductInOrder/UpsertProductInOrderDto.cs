using WebApi.Models.Product;

namespace WebApi.Models.ProductInOrder
{
    public class UpsertProductInOrderDto
    {
        public int Amount { get; set; }

        public int ProductId { get; set; }
    }
}
