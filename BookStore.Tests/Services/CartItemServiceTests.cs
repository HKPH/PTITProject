using AutoMapper;
using BookStore.Application.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BookStore.Application.Service;
using BookStore.Application.Entities;
using BookStore.Application.Dtos;

namespace BookStore.Tests.Services
{
    public class CartItemServiceTests
    {
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly CartItemService _cartItemService;

        public CartItemServiceTests()
        {
            _cartItemService = new CartItemService(_cartItemRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedCartItems()
        {
            // Arrange
            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 1, BookId = 1, CartId = 1, Quantity = 2 },
                new CartItem { Id = 2, BookId = 2, CartId = 1, Quantity = 3 }
            };
            var cartItemDtos = new List<CartItemDto>
            {
                new CartItemDto { Id = 1, BookId = 1, CartId = 1, Quantity = 2 },
                new CartItemDto { Id = 2, BookId = 2, CartId = 1, Quantity = 3 }
            };
            _cartItemRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cartItems);
            _mapper.Setup(m => m.Map<IEnumerable<CartItemDto>>(It.IsAny<IEnumerable<CartItem>>())).Returns(cartItemDtos);

            // Act
            var result = await _cartItemService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.First().BookId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsync_ValidId_ShouldReturnCartItem(int id)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            var mapperMock = new Mock<IMapper>();

            var cartItem = new CartItem { Id = id, CartId = 1, BookId = 101, Quantity = 2 };
            var cartItemDto = new CartItemDto { Id = id, CartId = 1, BookId = 101, Quantity = 2 };

            cartItemRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(cartItem);
            mapperMock.Setup(x => x.Map<CartItemDto>(It.IsAny<CartItem>())).Returns(cartItemDto);

            var service = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.Equal(id, result.Id);
        }


        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        public async Task GetByIdAsync_InvalidId_ShouldThrowException(int id)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((CartItem)null);

            var mapperMock = new Mock<IMapper>();

            var service = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetByIdAsync(id));
        }

        [Fact]
        public async Task CreateAsync_CartItemExists_ShouldIncrementQuantity()
        {
            // Arrange
            var cartItemDto = new CartItemDto
            {
                CartId = 1,
                BookId = 1,
                Quantity = 1
            };

            var existingCartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                BookId = 1,
                Quantity = 1
            };

            var updatedCartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                BookId = 1,
                Quantity = 2
            };

            var expectedDto = new CartItemDto
            {
                CartId = 1,
                BookId = 1,
                Quantity = 2
            };

            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(r => r.CartItemExistsAsync(cartItemDto.CartId, cartItemDto.BookId))
                .ReturnsAsync(true);
            cartItemRepositoryMock
                .Setup(r => r.GetCartItemsByCartIdAsync(cartItemDto.CartId))
                .ReturnsAsync(new List<CartItem> { existingCartItem });
            cartItemRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<CartItem>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            cartItemRepositoryMock
                .Setup(r => r.GetByIdAsync(existingCartItem.Id))
                .ReturnsAsync(updatedCartItem);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<CartItemDto>(updatedCartItem))
                .Returns(expectedDto);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await cartItemService.CreateAsync(cartItemDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Quantity);
        }


        [Fact]
        public async Task CreateAsync_CartItemNotExists_ShouldCreateNewItem()
        {
            // Arrange
            var cartItemDto = new CartItemDto
            {
                CartId = 1,
                BookId = 2,
                Quantity = 1
            };

            var newCartItem = new CartItem
            {
                Id = 2,
                CartId = 1,
                BookId = 2,
                Quantity = 1
            };

            var expectedDto = new CartItemDto
            {
                CartId = 1,
                BookId = 2,
                Quantity = 1
            };

            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(r => r.CartItemExistsAsync(cartItemDto.CartId, cartItemDto.BookId))
                .ReturnsAsync(false);
            cartItemRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<CartItem>()))
                .ReturnsAsync(newCartItem);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<CartItem>(cartItemDto))
                .Returns(newCartItem);
            mapperMock
                .Setup(m => m.Map<CartItemDto>(newCartItem))
                .Returns(expectedDto);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await cartItemService.CreateAsync(cartItemDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Quantity);
            Assert.Equal(2, result.BookId);
        }


        [Fact]
        public async Task CreateAsync_UpdateFails_ShouldThrowException()
        {
            // Arrange
            var cartItemDto = new CartItemDto
            {
                CartId = 1,
                BookId = 1,
                Quantity = 1
            };

            var existingCartItem = new CartItem
            {
                Id = 1,
                CartId = 1,
                BookId = 1,
                Quantity = 1
            };

            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.CartItemExistsAsync(cartItemDto.CartId, cartItemDto.BookId))
                .ReturnsAsync(true);
            cartItemRepositoryMock
                .Setup(x => x.GetCartItemsByCartIdAsync(cartItemDto.CartId))
                .ReturnsAsync(new List<CartItem> { existingCartItem });
            cartItemRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<CartItem>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<CartItemDto>(It.IsAny<CartItem>()))
                .Returns(cartItemDto);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cartItemService.CreateAsync(cartItemDto));
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        public async Task UpdateAsync_ValidId_ShouldUpdateCartItem(int id, int newQuantity)
        {
            // Arrange
            var cartItemDto = new CartItemDto { CartId = 1, BookId = 1, Quantity = newQuantity };
            var existingCartItem = new CartItem { Id = id, CartId = 1, BookId = 1, Quantity = 1 };

            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(existingCartItem);
            cartItemRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<CartItem>(), id))
                .ReturnsAsync(true);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<CartItem>(cartItemDto))
                .Returns(new CartItem { CartId = 1, BookId = 1, Quantity = newQuantity });

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await cartItemService.UpdateAsync(cartItemDto, id);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        public async Task UpdateAsync_InvalidId_ShouldThrowException(int id)
        {
            // Arrange
            var cartItemDto = new CartItemDto { CartId = 1, BookId = 1, Quantity = 10 };

            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((CartItem)null);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, new Mock<IMapper>().Object);

            // Act 
            var ex = await Assert.ThrowsAsync<Exception>(() => cartItemService.UpdateAsync(cartItemDto, id));

            //Assert
            Assert.Equal("CartItem not found.", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_ValidId_ShouldDeleteCartItem(int id)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(new CartItem { Id = id });

            cartItemRepositoryMock
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(true);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, new Mock<IMapper>().Object);

            // Act
            var result = await cartItemService.DeleteAsync(id);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        public async Task DeleteAsync_InvalidId_ShouldThrowException(int id)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((CartItem)null);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, new Mock<IMapper>().Object);

            // Act
            var ex  = await Assert.ThrowsAsync<Exception>(() => cartItemService.DeleteAsync(id));

            //Assert
            Assert.Equal("CartItem not found.", ex.Message);
        }

      
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCartItemsByCartIdAsync_ValidId_ShouldReturnItems(int cartId)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync(new List<CartItem> { new CartItem { CartId = cartId, BookId = 1, Quantity = 1 } });

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, new Mock<IMapper>().Object);

            // Act
            var result = await cartItemService.GetCartItemsByCartIdAsync(cartId);

            // Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(999)] 
        [InlineData(1000)]
        public async Task GetCartItemsByCartIdAsync_NullResult_ShouldThrowException(int cartId)
        {
            // Arrange
            var cartItemRepositoryMock = new Mock<ICartItemRepository>();
            cartItemRepositoryMock
                .Setup(x => x.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync((List<CartItem>)null);

            var cartItemService = new CartItemService(cartItemRepositoryMock.Object, new Mock<IMapper>().Object);

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => cartItemService.GetCartItemsByCartIdAsync(cartId));

            // Assert
            Assert.Equal("CartItems not found.", ex.Message);
        }



    }
}
