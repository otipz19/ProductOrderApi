using Microsoft.AspNetCore.Mvc;
using WebApi.Enums;
using WebApi.Models.OrderStatus;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrderStatusController : ControllerBase
    {
        /// <summary>
        /// Returns list of all order statuses
        /// </summary>
        /// <response code="200">Returns list of all order statuses</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
