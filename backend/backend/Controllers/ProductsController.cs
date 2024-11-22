using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using backend.Dtos; 
using AutoMapper;
using backend.Dtos;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepo<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IRepo<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
        {
            var products = await _productRepo.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductReadDto>>(products);
            return Ok(productsDto);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductReadDto>(product);
            return Ok(productDto);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto productDto)
        {
            var existingProduct = await _productRepo.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Map DTO to Entity
            _mapper.Map(productDto, existingProduct);

            bool updated = await _productRepo.UpdateAsync(existingProduct);
            if (!updated)
            {
                return BadRequest("Unable to update the product.");
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product is required.");
            }

            // Map DTO to Entity
            var product = _mapper.Map<Product>(productDto);

            var added = await _productRepo.AddAsync(product);
            if (added == null)
            {
                return BadRequest("Unable to add the product.");
            }

            return CreatedAtAction("GetProduct", new { id = added.ProductID }, added);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            bool deleted = await _productRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the product.");
            }

            return NoContent();
        }
    }
}
