using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        public virtual List<Order> Orders { get; set; } = new();
    }
}
