using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Order;

namespace WebApi.MappingProfiles
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order, GetOrderDto>();
            CreateMap<UpsertOrderDto, Order>();
        }
    }
}
