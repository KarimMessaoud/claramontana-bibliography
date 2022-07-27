using ClaramontanaOnlineShop.Data.Entities;
using ClaramontanaOnlineShop.Service.ProductService;
using ClaramontanaOnlineShop.WebApi.Controllers;
using ClaramontanaOnlineShop.WebApi.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClaramontanaOnlineShop.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> productServiceStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            //Arrange
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            // Act
            var result = await controller.GetProductAsync(Guid.NewGuid());

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async Task GetProductAsync_WithExistingProduct_ReturnsExpectedProduct()
        {
            //Arange
            var expectedProduct = CreateRandomProduct();

            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProduct);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.GetProductAsync(Guid.NewGuid());

            //Assert
            result.Value.Should().BeEquivalentTo(expectedProduct);
        }


        [Fact]
        public async Task GetAllProductsAsync_WithExistingProducts_ReturnsAllProducts()
        {
            //Arrange
            var expectedProducts = new[] { CreateRandomProduct(), CreateRandomProduct(), CreateRandomProduct() };

            productServiceStub.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(expectedProducts);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var actualProducts = await controller.GetAllProductsAsync();

            //Assert
            actualProducts.Should().BeEquivalentTo(expectedProducts);
        }


        [Fact]
        public async Task CreateProductAsync_WithProductToCreate_ReturnsCreatedProduct()
        {
            //Arrange
            var productToCreate = new CreateProductDto
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                Image = Guid.NewGuid().ToString(),
                Quantity = rand.Next(1000),
                IsAvailable = true
            };

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.CreateProductAsync(productToCreate);

            //Assert
            var createdProduct = (result.Result as CreatedAtActionResult).Value as ProductDto;

            productToCreate.Should().BeEquivalentTo(createdProduct,
                options => options.ExcludingMissingMembers());
        }


        [Fact]
        public async Task UpdateProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            //Arrange
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            var productToUpdate = new UpdateProductDto
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                Image = Guid.NewGuid().ToString(),
                Quantity = rand.Next(1000),
                IsAvailable = false
            };

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.UpdateProductAsync(Guid.NewGuid(), productToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async Task UpdateProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            //Arrange
            var existingProduct = CreateRandomProduct();
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var productId = existingProduct.Id;
            var productToUpdate = new UpdateProductDto
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = existingProduct.Price + 3,
                Image = Guid.NewGuid().ToString(),
                Quantity = rand.Next(1000),
                IsAvailable = false
            };

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.UpdateProductAsync(productId, productToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            //Arrange
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.DeleteProductAsync(Guid.NewGuid());

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            //Arrange
            var existingProduct = CreateRandomProduct();
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.DeleteProductAsync(existingProduct.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PatchProductAsync_WithPatchDocumentNull_ReturnsBadRequest()
        {
            //Arrange
            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.PatchProductAsync(Guid.NewGuid(), null);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task PatchProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            //Arrange
            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);

            //Act
            var result = await controller.PatchProductAsync(Guid.NewGuid(), new JsonPatchDocument<Product>());

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PatchProductAsync_WithExistingProduct_ReturnsObjectResult()
        {
            //Arrange
            var existingProduct = CreateRandomProduct();

            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);
            var productId = existingProduct.Id;

            var patch = new JsonPatchDocument<Product>();
            patch.Replace(x => x.Name, Guid.NewGuid().ToString());
            patch.Replace(x => x.Price, rand.Next(1000));


            //Act
            var result = await controller.PatchProductAsync(productId, patch);

            //Assert
            result.Should().BeOfType<ObjectResult>();

            var updatedProduct = (result as ObjectResult).Value as UpdateProductDto;
            updatedProduct.Name.Should().BeEquivalentTo(patch.Operations[0].value.ToString());
            updatedProduct.Price.Should().Be((decimal)patch.Operations[1].value);
        }

        [Fact]
        public async Task PatchProductAsync_WithInvalidModelState_ReturnsBadRequest()
        {
            //Arrange
            var existingProduct = CreateRandomProduct();

            productServiceStub.Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var controller = new ProductsController(productServiceStub.Object, ObjectMapper.Mapper);
            var productId = existingProduct.Id;

            var patch = new JsonPatchDocument<Product>();

            //null is invalid for quantity
            patch.Operations.Add(new Operation<Product>("replace", "/quantity", null));

            //Act
            var result = await controller.PatchProductAsync(productId, patch);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        private Product CreateRandomProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                Image = Guid.NewGuid().ToString(),
                Quantity = rand.Next(1000),
                IsAvailable = true
            };
        }
    }
}
