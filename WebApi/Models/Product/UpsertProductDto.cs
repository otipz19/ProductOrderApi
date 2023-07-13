using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Product
{
    public class UpsertProductDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }
    }
}
