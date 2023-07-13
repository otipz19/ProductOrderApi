using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            Product product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            Product toUpdate = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (toUpdate is null)
                return NotFound();

            _context.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Product toDelete = await _context.Products
                .FindAsync(id);

            if(toDelete is null)
                return NotFound();

            _context.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
