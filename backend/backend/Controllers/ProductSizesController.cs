using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using AutoMapper;
using backend.Dtos;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IRepo<ProductSize> _productSizeRepo;
        private readonly IMapper _mapper;

        public ProductSizesController(IRepo<ProductSize> productSizeRepo, IMapper mapper)
        {
            _productSizeRepo = productSizeRepo;
            _mapper = mapper;
        }

        // GET: api/ProductSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSizeReadDto>>> GetProductSizes()
        {
            var productSizes = await _productSizeRepo.GetAllAsync();
            var productSizesDto = _mapper.Map<IEnumerable<ProductSizeReadDto>>(productSizes);
            return Ok(productSizesDto);
        }

        // GET: api/ProductSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSizeReadDto>> GetProductSize(int id)
        {
            var productSize = await _productSizeRepo.GetByIdAsync(id);

            if (productSize == null)
            {
                return NotFound();
            }

            var productSizeDto = _mapper.Map<ProductSizeReadDto>(productSize);
            return Ok(productSizeDto);
        }

        // PUT: api/ProductSizes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSize(int id, ProductSizeUpdateDto productSize)
        {
            var existingProductSize = await _productSizeRepo.GetByIdAsync(id);
            if (existingProductSize == null)
            {
                return NotFound();
            }

            var productSizeEntity = _mapper.Map<ProductSize>(productSize);
            bool updated = await _productSizeRepo.UpdateAsync(productSizeEntity);
            if (!updated)
            {
                return BadRequest("Unable to update the product size.");
            }

            return NoContent();
        }

        // POST: api/ProductSizes
        [HttpPost]
        public async Task<ActionResult<ProductSize>> PostProductSize(ProductSizeCreateDto productSize)
        {
            var productSizeEntity = _mapper.Map<ProductSize>(productSize);
            var added = await _productSizeRepo.AddAsync(productSizeEntity);
            if (added == null)
            {
                return BadRequest("Unable to add the product size.");
            }

            return CreatedAtAction("GetProductSize", new { id = added.ProductSizeID }, added);
        }

        // DELETE: api/ProductSizes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSize(int id)
        {
            var productSize = await _productSizeRepo.GetByIdAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }

            bool deleted = await _productSizeRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the product size.");
            }

            return NoContent();
        }
    }
}
