using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class ProductInOrder
    {
        [Key]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Key]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Amount { get; set; }
    }
}
