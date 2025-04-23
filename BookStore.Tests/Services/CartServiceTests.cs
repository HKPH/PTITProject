using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Service;
using Moq;
namespace BookStore.Tests.Services;
public class CartServiceTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _mapperMock = new Mock<IMapper>();
        _cartService = new CartService(_cartRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCarts()
    {
        // Arrange
        var carts = new List<Cart> { new Cart { Id = 1, UserId = 1 } };
        var cartDtos = new List<CartDto> { new CartDto { Id = 1, UserId = 1 } };

        _cartRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(carts);
        _mapperMock.Setup(m => m.Map<IEnumerable<CartDto>>(carts)).Returns(cartDtos);

        // Act
        var result = await _cartService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(999)]
    public async Task GetByIdAsync_CartNotFound_ShouldThrowException(int id)
    {
        // Arrange
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Cart)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cartService.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByIdAsync_CartFound_ShouldReturnDto()
    {
        // Arrange
        var cart = new Cart { Id = 1, UserId = 1 };
        var cartDto = new CartDto { Id = 1, UserId = 1 };

        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cart.Id)).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartDto>(cart)).Returns(cartDto);

        // Act
        var result = await _cartService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedCartDto()
    {
        // Arrange
        var cartDto = new CartDto { Id = 1, UserId = 1 };
        var cart = new Cart { Id = 1, UserId = 1 };

        _mapperMock.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);
        _cartRepositoryMock.Setup(r => r.CreateAsync(cart)).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartDto>(cart)).Returns(cartDto);

        // Act
        var result = await _cartService.CreateAsync(cartDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartDto.Id, result.Id);
    }

    [Fact]
    public async Task CreateAsync_FailedToCreate_ShouldThrowException()
    {
        // Arrange
        var cartDto = new CartDto { Id = 1, UserId = 1 };
        var cart = new Cart { Id = 1, UserId = 1 };

        _mapperMock.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);
        _cartRepositoryMock.Setup(r => r.CreateAsync(cart)).ReturnsAsync((Cart)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cartService.CreateAsync(cartDto));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(1000)]
    public async Task UpdateAsync_CartNotFound_ShouldThrowException(int id)
    {
        // Arrange
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Cart)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cartService.UpdateAsync(new CartDto(), id));
    }

    [Fact]
    public async Task UpdateAsync_CartFound_ShouldUpdateSuccessfully()
    {
        // Arrange
        var id = 1;
        var cartDto = new CartDto { Id = id, UserId = 1 };
        var cart = new Cart { Id = id, UserId = 1 };

        _cartRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);
        _cartRepositoryMock.Setup(r => r.UpdateAsync(cart, id)).ReturnsAsync(true);

        // Act
        var result = await _cartService.UpdateAsync(cartDto, id);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(9999)]
    public async Task DeleteAsync_CartNotFound_ShouldThrowException(int id)
    {
        // Arrange
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Cart)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _cartService.DeleteAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_CartFound_ShouldDeleteSuccessfully()
    {
        // Arrange
        var id = 1;
        var cart = new Cart { Id = id, UserId = 1 };

        _cartRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(cart);
        _cartRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _cartService.DeleteAsync(id);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetCartByUserIdAsync_ShouldReturnCartDto(int userId)
    {
        // Arrange
        var cart = new Cart { Id = 1, UserId = userId };
        var cartDto = new CartDto { Id = 1, UserId = userId };

        _cartRepositoryMock.Setup(r => r.GetCartByUserIdAsync(userId)).ReturnsAsync(cart);
        _mapperMock.Setup(m => m.Map<CartDto>(cart)).Returns(cartDto);

        // Act
        var result = await _cartService.GetCartByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
    }
}
