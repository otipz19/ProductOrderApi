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
    [Produces("application/json")]
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

        /// <summary>
        /// Returns list of all products
        /// </summary>
        /// <response code="200">Returns list of all products</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAll()
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();
            return products
                .Select(_mapper.Map<GetProductDto>)
                .ToList();
        }

        /// <summary>
        /// Returns product with specified id
        /// </summary>
        /// <response code="200">Returns product with specified id</response>
        /// <response code="404">If product with specified id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetProductDto>> Get(int id)
        {
            Product product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound();

            return _mapper.Map<GetProductDto>(product);
        }

        /// <summary>
        /// Creates new product and returns it
        /// </summary>
        /// <response code="201">Returns newly created product</response>
        /// <response code="400">If validation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProductDto>> Post(UpsertProductDto upsertDto)
        {
            Product product = _mapper.Map<Product>(upsertDto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            GetProductDto getDto = _mapper.Map<GetProductDto>(product);
            return CreatedAtAction(nameof(Get), new { id = getDto.Id }, getDto);
        }

        /// <summary>
        /// Updates product with specified id
        /// </summary>
        /// <response code="204">If successed</response>
        /// <response code="400">If validation failed</response>
        /// <response code="404">If product with specified id does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, UpsertProductDto upsertDto)
        {
            Product toUpdate = await _context.Products.FindAsync(id);

            if (toUpdate is null)
                return NotFound();

            _mapper.Map(upsertDto, toUpdate);

            _context.Products.Update(toUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes product with specified id
        /// </summary>
        /// <response code="204">If successed</response>
        /// <response code="404">If product with specified id does not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
