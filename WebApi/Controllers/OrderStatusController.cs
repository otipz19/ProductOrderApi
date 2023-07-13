using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebApi.Enums;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<OrderStatus>> GetAll()
        {
            return Enum.GetValues<OrderStatus>(); 
        }
    }
}
