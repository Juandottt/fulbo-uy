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
    public async Task CreateProduct_ShouldSucceed_WhenValid()
    {
        // Arrange
        const string name = "Water Bottle";
        const decimal price = 150m;

        _productRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product product) => product);

        var service = CreateService();

        // Act
        var result = await service.CreateAsync(name, price);

        // Assert
        result.Name.Should().Be(name);
        result.Price.Should().Be(price);
        result.IsActive.Should().BeTrue();
        _productRepository.Verify(repository => repository.CreateAsync(
            It.Is<Product>(product =>
                product.Name == name &&
                product.Price == price &&
                product.IsActive)), Times.Once);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.CreateAsync("", 150m);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldFail_WhenPriceIsNotPositive()
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.CreateAsync("Water Bottle", 0m);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        _productRepository.Verify(repository => repository.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [TestMethod]
    public async Task GetProductById_ShouldSucceed_WhenFound()
    {
        // Arrange
        var product = CreateProduct();
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var service = CreateService();

        // Act
        var result = await service.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result.Name.Should().Be(product.Name);
        result.Price.Should().Be(product.Price);
        result.IsActive.Should().Be(product.IsActive);
    }

    [TestMethod]
    public async Task GetProductById_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepository.Setup(repository => repository.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        var service = CreateService();

        // Act
        var result = await service.GetByIdAsync(productId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetActiveProducts_ShouldReturnActiveProducts()
    {
        // Arrange
        var products = new[]
        {
            CreateProduct(name: "Water Bottle"),
            CreateProduct(name: "Energy Bar")
        };

        _productRepository.Setup(repository => repository.GetActiveAsync()).ReturnsAsync(products);

        var service = CreateService();

        // Act
        var result = await service.GetActiveAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(product => product.IsActive);
        result.Select(product => product.Name).Should().BeEquivalentTo("Water Bottle", "Energy Bar");
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldSucceed_WhenValid()
    {
        // Arrange
        var product = CreateProduct(name: "Water Bottle", price: 150m);
        const string updatedName = "Sports Drink";
        const decimal updatedPrice = 200m;

        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _productRepository
            .Setup(repository => repository.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product updatedProduct) => updatedProduct);

        var service = CreateService();

        // Act
        var result = await service.UpdateAsync(product.Id, updatedName, updatedPrice);

        // Assert
        result.Id.Should().Be(product.Id);
        result.Name.Should().Be(updatedName);
        result.Price.Should().Be(updatedPrice);
        result.IsActive.Should().BeTrue();
    }

    [TestMethod]
    public async Task DeleteProduct_ShouldSoftDelete_WhenFound()
    {
        // Arrange
        var product = CreateProduct();
        _productRepository.Setup(repository => repository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _productRepository
            .Setup(repository => repository.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product updatedProduct) => updatedProduct);

        var service = CreateService();

        // Act
        await service.DeleteAsync(product.Id);

        // Assert
        _productRepository.Verify(repository => repository.UpdateAsync(
            It.Is<Product>(updatedProduct =>
                updatedProduct.Id == product.Id &&
                !updatedProduct.IsActive)), Times.Once);
    }

    private ProductService CreateService()
    {
        return new ProductService(_productRepository.Object);
    }

    private static Product CreateProduct(
        Guid? id = null,
        string name = "Water Bottle",
        decimal price = 150m,
        bool isActive = true)
    {
        return new Product
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Price = price,
            IsActive = isActive
        };
    }
}
