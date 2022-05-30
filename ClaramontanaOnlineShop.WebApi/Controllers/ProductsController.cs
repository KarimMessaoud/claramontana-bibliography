using AutoMapper;
using ClaramontanaOnlineShop.Data.Entities;
using ClaramontanaOnlineShop.Service.ProductService;
using ClaramontanaOnlineShop.WebApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = _mapper.Map<IEnumerable<ProductDto>>(await _productService.GetAllProductsAsync());

            return products;
        }


        [ActionName("GetProductAsync")] //This attribute is needed for the proper link generation in CreateProduct method,
                                     //because by default the Async suffix is trimmed
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }


        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            await _productService.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id }, _mapper.Map<CreateProductDto>(product));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);

            if(existingProduct == null)
            {
                return NotFound();
            }

            Product updatedProduct = _mapper.Map<Product>(productDto);
            updatedProduct.Id = existingProduct.Id;

            await _productService.UpdateProductAsync(updatedProduct);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }


        [HttpPatch("{productId:guid}")]
        public async Task<ActionResult> PatchProductAsync(Guid productId, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if(patchDoc != null)
            {
                var product = await _productService.GetProductByIdAsync(productId);

                if (product == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(product, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _productService.UpdateProductAsync(product);

                UpdateProductDto productDto = _mapper.Map<UpdateProductDto>(product);

                return new ObjectResult(productDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
