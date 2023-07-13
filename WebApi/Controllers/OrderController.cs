using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Models.Order;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderController(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns list of all order orders
        /// </summary>
        /// <response code="200">Returns list of all orders</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAll()
        {
            List<Order> orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.ProductsInOrder)
                    .ThenInclude(p => p.Product)
                .ToListAsync();

            return orders.Select(_mapper.Map<GetOrderDto>).ToList();
        }

        /// <summary>
        /// Returns order with specified id
        /// </summary>
        /// <response code="200">Returns order with specified id</response>
        /// <response code="404">If order with specified id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetOrderDto>> Get(int id)
        {
            Order order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.ProductsInOrder)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return NotFound();

            return _mapper.Map<GetOrderDto>(order);
        }

        /// <summary>
        /// Creates order with specified list of products and returns it
        /// </summary>
        /// <response code="200">Returns newly created order</response>
        /// <response code="400">If validation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetOrderDto>> Post(UpsertOrderDto upsertDto)
        {
            Order order = new Order()
            {
                Status = OrderStatus.Pending,
                StatusChangedAt = DateTime.Now,
            };

            await SetProductInOrderListFromDto(upsertDto, order);
            SetOrdetTotal(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            GetOrderDto getDto = _mapper.Map<GetOrderDto>(order);
            return CreatedAtAction(nameof(Get), new { id = getDto.Id }, getDto);
        }

        /// <summary>
        /// Updates list of products in order with specified id
        /// </summary>
        /// <response code="204">If successed</response>
        /// <response code="400">If validation failed</response>
        /// <response code="404">If order with specified id does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, UpsertOrderDto upsertDto)
        {
            Order toUpdate = await _context.Orders
                .Include(o => o.ProductsInOrder)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (toUpdate is null)
                return NotFound();

            _context.ProductsInOrders.RemoveRange(toUpdate.ProductsInOrder);
            toUpdate.ProductsInOrder.Clear();

            await SetProductInOrderListFromDto(upsertDto, toUpdate);
            SetOrdetTotal(toUpdate);

            _context.Orders.Update(toUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes order with specified id
        /// </summary>
        /// <response code="204">If successed</response>
        /// <response code="404">If order with specified id does not exist</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            Order toDelete = await _context.Orders.FindAsync(id);

            if (toDelete is null)
                return NotFound();

            _context.Orders.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates status of order with specified id
        /// </summary>
        /// <response code="204">If successed</response>
        /// <response code="400">If order status with specified id does not exist</response>
        /// <response code="404">If order with specified id does not exist</response>
        [HttpPut("{id}/{statusId}")]
        public async Task<ActionResult> ChangeStatus(int id, int statusId)
        {
            Order toUpdate = await _context.Orders.FindAsync(id);

            if (toUpdate is null)
                return NotFound();

            OrderStatus status;
            try
            {
                status = (OrderStatus)statusId;
            }
            catch
            {
                return BadRequest();
            }

            toUpdate.Status = status;
            toUpdate.StatusChangedAt = DateTime.Now;

            _context.Orders.Update(toUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void SetOrdetTotal(Order order)
        {
            order.OrderTotal = order.ProductsInOrder.Sum(p => p.Amount * p.Product.Price);
        }

        private async Task SetProductInOrderListFromDto(UpsertOrderDto upsertDto, Order order)
        {
            List<int> productIds = upsertDto.ProductsInOrder.Select(p => p.ProductId).ToList();
            List<Product> products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var product in products)
            {
                order.ProductsInOrder.Add(new ProductInOrder()
                {
                    Amount = upsertDto.ProductsInOrder.First(p => p.ProductId == product.Id).Amount,
                    ProductId = product.Id,
                    Product = product,
                    Order = order,
                });
            }
        }
    }
}
