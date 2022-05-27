using ClaramontanaOnlineShop.Data.Entities;
using ClaramontanaOnlineShop.Service;
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
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = (await _productService.GetAllProductsAsync()).Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Image = x.Image,
                Quantity = x.Quantity,
                IsAvailable = x.IsAvailable
            });

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

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
                Quantity = product.Quantity,
                IsAvailable = product.IsAvailable
            };

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            Product product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Image = productDto.Image,
                Quantity = productDto.Quantity,
                IsAvailable = productDto.IsAvailable
            };

            await _productService.CreateProductAsync(product);

            
            return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id },
                new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Image = productDto.Image,
                    Quantity = productDto.Quantity,
                    IsAvailable = productDto.IsAvailable
                });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);

            if(existingProduct == null)
            {
                return NotFound();
            }

            var updatedProduct = new Product
            {
                Id = existingProduct.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Image = productDto.Image,
                Quantity = productDto.Quantity,
                IsAvailable = productDto.IsAvailable
            };

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

                var productDto = new UpdateProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = product.Quantity,
                    IsAvailable = product.IsAvailable
                };

                return new ObjectResult(productDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
