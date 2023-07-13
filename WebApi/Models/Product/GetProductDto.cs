using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Product
{
    public class GetProductDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
