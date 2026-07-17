using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Domain.Entities;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepository = new();

    [TestMethod]
    public async Task CreateAsync_ShouldCreateActiveProduct_WhenDataIsValid()
    {
        var beforeCreation = DateTime.UtcNow;
        _productRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product product) => product);
        var service = CreateService();

        var result = await service.CreateAsync("Pelota", "Pelota profesional", 1250m);

        result.Name.Should().Be("Pelota");
        result.Description.Should().Be("Pelota profesional");
        result.Price.Should().Be(1250m);
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        _productRepository.Verify(repository => repository.CreateAsync(It.Is<Product>(product =>
            product.Name == "Pelota" &&
            product.Description == "Pelota profesional" &&
            product.Price == 1250m &&
            product.IsActive)), Times.Once);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldUseEmptyDescription_WhenDescriptionIsNull()
    {
        _productRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product product) => product);
        var service = CreateService();

        var result = await service.CreateAsync("Pelota", null, 1250m);

        result.Description.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public async Task CreateAsync_ShouldFail_WhenNameIsBlank(string name)
    {
        var service = CreateService();

        var act = () => service.CreateAsync(name, null, 1250m);

        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task CreateAsync_ShouldFail_WhenPriceIsNotPositive(int price)
    {
        var service = CreateService();

        var act = () => service.CreateAsync("Pelota", null, price);

        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        _productRepository.Setup(repository => repository.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
        var service = CreateService();

        var result = await service.GetByIdAsync(productId);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var product = CreateProduct("Pelota", 1250m);
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        var service = CreateService();

        var result = await service.GetByIdAsync(product.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result.Name.Should().Be("Pelota");
    }

    [TestMethod]
    public async Task GetActiveAsync_ShouldReturnOnlyProductsProvidedByActiveQuery()
    {
        var products = new[]
        {
            CreateProduct("Pelota", 1250m),
            CreateProduct("Conos", 600m)
        };
        _productRepository.Setup(repository => repository.GetActiveAsync()).ReturnsAsync(products);
        var service = CreateService();

        var result = await service.GetActiveAsync();

        result.Should().HaveCount(2);
        result.Should().OnlyContain(product => product.IsActive);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenDataIsValid()
    {
        var product = CreateProduct("Pelota", 1250m);
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _productRepository
            .Setup(repository => repository.UpdateAsync(product))
            .ReturnsAsync((Product updatedProduct) => updatedProduct);
        var service = CreateService();

        var result = await service.UpdateAsync(product.Id, "Pelota Pro", "Edición actualizada", 1500m);

        result.Name.Should().Be("Pelota Pro");
        result.Description.Should().Be("Edición actualizada");
        result.Price.Should().Be(1500m);
        _productRepository.Verify(repository => repository.UpdateAsync(product), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldFail_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        _productRepository.Setup(repository => repository.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
        var service = CreateService();

        var act = () => service.UpdateAsync(productId, "Pelota", string.Empty, 1250m);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _productRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public async Task UpdateAsync_ShouldFail_WhenNameIsBlank(string name)
    {
        var product = CreateProduct("Pelota", 1250m);
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        var service = CreateService();

        var act = () => service.UpdateAsync(product.Id, name, null, 1500m);

        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task UpdateAsync_ShouldFail_WhenPriceIsNotPositive(int price)
    {
        var product = CreateProduct("Pelota", 1250m);
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        var service = CreateService();

        var act = () => service.UpdateAsync(product.Id, "Pelota", null, price);

        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldDeactivateProduct_WhenProductExists()
    {
        var product = CreateProduct("Pelota", 1250m);
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _productRepository.Setup(repository => repository.UpdateAsync(product)).ReturnsAsync(product);
        var service = CreateService();

        await service.DeleteAsync(product.Id);

        product.IsActive.Should().BeFalse();
        _productRepository.Verify(repository => repository.UpdateAsync(product), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldFail_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        _productRepository.Setup(repository => repository.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
        var service = CreateService();

        var act = () => service.DeleteAsync(productId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _productRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    private ProductService CreateService() => new(_productRepository.Object);

    private static Product CreateProduct(string name, decimal price) => new()
    {
        Name = name,
        Price = price
    };
}
