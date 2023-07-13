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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetAll()
        {
            List<Order> orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.ProductsInOrder)
                    .ThenInclude(p => p.Product)
                .ToListAsync();

            return orders.Select(_mapper.Map<GetOrderDto>).ToList();
        }

        [HttpGet("{id}")]
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

        [HttpPost]
        public async Task<ActionResult<GetOrderDto>> Post(UpsertOrderDto upsertDto)
        {
            Order order = _mapper.Map<Order>(upsertDto);

            order.Status = OrderStatus.Pending;
            order.StatusChangedAt = DateTime.Now;
            SetOrdetTotal(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            GetOrderDto getDto = _mapper.Map<GetOrderDto>(order);
            return CreatedAtAction(nameof(Get), new { id = getDto.Id }, getDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, UpsertOrderDto upsertDto)
        {
            Order toUpdate = await _context.Orders.FindAsync(id);

            if(toUpdate is null)
                return NotFound();

            _mapper.Map(upsertDto, toUpdate);
            SetOrdetTotal(toUpdate);

            _context.Update(toUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            Order toDelete = await _context.Orders.FindAsync(id);

            if (toDelete is null)
                return NotFound();

            _context.Orders.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void SetOrdetTotal(Order order)
        {
            order.OrderTotal = order.ProductsInOrder.Sum(p => p.Amount * p.Product.Price);
        }
    }
}
