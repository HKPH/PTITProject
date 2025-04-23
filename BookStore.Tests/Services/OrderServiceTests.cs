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
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IOrderItemRepository> _orderItemRepository = new();
        private readonly Mock<IShipmentRepository> _shipmentRepository = new();
        private readonly Mock<IPaymentRepository> _paymentRepository = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IBookRepository> _bookRepository = new();
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _orderRepository.Object,
                _orderItemRepository.Object,
                _shipmentRepository.Object,
                _paymentRepository.Object,
                _cartItemRepository.Object,
                _cartRepository.Object,
                _bookRepository.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_OrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            var orderDto = new OrderDto { Id = orderId };
            _orderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);
            // Act
            var result = await _orderService.GetByIdAsync(orderId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_OrderDoesNotExist_ThrowsException()
        {
            // Arrange
            var orderId = 1;
            _orderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.GetByIdAsync(orderId));

            //Assert
            Assert.Equal("Order not found.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_CartItemsExistAndStockSufficient_CreatesOrder()
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto();
            var paymentDto = new PaymentDto();
            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
            {
                new CartItem { BookId = 1, Quantity = 2 },
                new CartItem { BookId = 2, Quantity = 3 }
            };
            var book1 = new Book { Id = 1, Price = 10, StockQuantity = 5 };
            var book2 = new Book { Id = 2, Price = 20, StockQuantity = 10 };
            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book1);
            _bookRepository.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(book2);
            // Act
            var result = await _orderService.CreateAsync(orderDto, shipmentDto, paymentDto);
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_EmptyCart_ThrowsException()
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto();
            var paymentDto = new PaymentDto();
            var cart = new Cart { Id = 1, UserId = 1 };
            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(new List<CartItem>());
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDto, shipmentDto, paymentDto));
            // Assert
            Assert.Equal("Cart is empty.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ShipmentCreationFails_ThrowsException()
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto { ShippingAddressId = 123 };
            var paymentDto = new PaymentDto { PaymentMethod = "Credit Card" };

            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 1, BookId = 1, Quantity = 2 }
            };
            var book = new Book { Id = 1, Price = 10, StockQuantity = 10 };

            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _shipmentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync((Shipment?)null);

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDto, shipmentDto, paymentDto));

            // Assert
            Assert.Equal("Failed to create Shipment.", ex.Message);
        }


        [Fact]
        public async Task CreateAsync_PaymentCreationFails_ThrowsException()
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto();
            var paymentDto = new PaymentDto();
            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 1, BookId = 1, Quantity = 2 }
            };
            var book = new Book { Id = 1, Price = 10, StockQuantity = 10 };
            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _shipmentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync(new Shipment());
            _paymentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Payment>())).ReturnsAsync((Payment?)null);
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDto, shipmentDto, paymentDto));
            // Assert
            Assert.Equal("Failed to create Payment.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_OrderCreationFails_ThrowsException()
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto();
            var paymentDto = new PaymentDto();
            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 1, BookId = 1, Quantity = 2 }
            };
            var book = new Book { Id = 1, Price = 10, StockQuantity = 10 };
            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _shipmentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync(new Shipment());
            _paymentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment());
            _orderRepository.Setup(repo => repo.CreateAsync(It.IsAny<Order>())).ReturnsAsync((Order?)null);
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDto, shipmentDto, paymentDto));
            // Assert
            Assert.Equal("Failed to create Order.", ex.Message);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(2, 5)]
        public async Task CreateAsync_BookStockInsufficient_ThrowsException(int bookStock, int quantity)
        {
            // Arrange
            var orderDto = new OrderDto { UserId = 1 };
            var shipmentDto = new ShipmentDto();
            var paymentDto = new PaymentDto();
            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
            {
                new CartItem { BookId = 1, Quantity = quantity }
            };
            var book = new Book { Id = 1, Price = 10, StockQuantity = bookStock };
            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _shipmentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync(new Shipment());
            _paymentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment());
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(orderDto, shipmentDto, paymentDto));
            // Assert
            Assert.Equal("Not enough stock", ex.Message);
        }
        [Fact]
        public async Task UpdateAsync_OrderExists_UpdatesOrder()
        { }

        [Fact]
        public async Task UpdateAsync_OrderDoesNotExist_ThrowsException()
        { }
        [Fact]
        public async Task DeleteAsync_OrderExists_DeletesOrder()
        { }

        [Fact]
        public async Task DeleteAsync_OrderDoesNotExist_ThrowsException()
        { }
        [Fact]
        public async Task GetAllAsync_ValidRequest_ReturnsPaginatedOrdersWithItems()
        { }
        [Fact]
        public async Task GetOrdersByUserIdAsync_ValidUserAndStatus_ReturnsOrdersWithItems()
        { }
        [Fact]
        public async Task UpdateStatusAsync_OrderExists_UpdatesStatus()
        { }

        [Fact]
        public async Task UpdateStatusAsync_StatusIsThree_UpdatesShipmentDate()
        { }

        [Fact]
        public async Task UpdateStatusAsync_OrderDoesNotExist_ThrowsException()
        { }


    }
}
