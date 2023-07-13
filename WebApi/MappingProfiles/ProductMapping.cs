using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Product;

namespace WebApi.MappingProfiles
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<UpsertProductDto, Product>();
            CreateMap<Product, GetProductDto>();
        }
    }
}
