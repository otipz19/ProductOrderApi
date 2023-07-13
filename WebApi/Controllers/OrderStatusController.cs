using Microsoft.AspNetCore.Mvc;
using WebApi.Enums;
using WebApi.Models.OrderStatus;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<GetOrderStatus>> GetAll()
        {
            return Enum.GetValues<OrderStatus>()
                .Select(e => new GetOrderStatus()
                {
                    Id = (int)e,
                    Name = e.ToString(),
                })
                .ToList(); 
        }
    }
}
