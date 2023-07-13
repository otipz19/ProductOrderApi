using AutoMapper;
using WebApi.Entities;
using WebApi.Models.ProductInOrder;

namespace WebApi.MappingProfiles
{
    public class ProductInOrderMapping : Profile
    {
        public ProductInOrderMapping()
        {
            CreateMap<ProductInOrder, GetProductInOrderDto>();
            CreateMap<UpsertProductInOrderDto, ProductInOrder>();
        }
    }
}
