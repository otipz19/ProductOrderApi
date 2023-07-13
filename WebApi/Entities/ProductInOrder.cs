using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class ProductInOrder
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Amount { get; set; }
    }
}
