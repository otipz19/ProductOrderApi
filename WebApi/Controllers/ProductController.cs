using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Models.Product;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAll()
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();
            return products
                .Select(_mapper.Map<GetProductDto>)
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> Get(int id)
        {
            Product product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound();

            return _mapper.Map<GetProductDto>(product);
        }

        [HttpPost]
        public async Task<ActionResult<GetProductDto>> Post(UpsertProductDto upsertDto)
        {
            if (upsertDto is null)
                return BadRequest();

            Product product = _mapper.Map<Product>(upsertDto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            GetProductDto getDto = _mapper.Map<GetProductDto>(product);
            return CreatedAtAction(nameof(Get), new { id = getDto.Id }, getDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpsertProductDto upsertDto)
        {
            if (upsertDto is null)
                return BadRequest();

            Product toUpdate = await _context.Products.FindAsync(id);

            if (toUpdate is null)
                return NotFound();

            _mapper.Map(upsertDto, toUpdate);

            _context.Products.Update(toUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Product toDelete = await _context.Products.FindAsync(id);

            if(toDelete is null)
                return NotFound();

            _context.Products.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
