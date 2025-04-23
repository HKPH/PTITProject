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
            var shipmentDto = new ShipmentDto { ShippingAddressId = 100 };
            var paymentDto = new PaymentDto { PaymentMethod = "Credit Card" };

            var cart = new Cart { Id = 1, UserId = 1 };
            var cartItems = new List<CartItem>
    {
        new CartItem { Id = 1, BookId = 1, Quantity = 2 },
        new CartItem { Id = 2, BookId = 2, Quantity = 3 }
    };
            var book1 = new Book { Id = 1, Price = 10, StockQuantity = 5 };
            var book2 = new Book { Id = 2, Price = 20, StockQuantity = 10 };

            var shipment = new Shipment { Id = 1 };
            var payment = new Payment { Id = 1 };
            var order = new Order { Id = 1, UserId = 1, ShipmentId = 1, PaymentId = 1 };

            _cartRepository.Setup(repo => repo.GetCartByUserIdAsync(orderDto.UserId)).ReturnsAsync(cart);
            _cartItemRepository.Setup(repo => repo.GetCartItemsByCartIdAsync(cart.Id)).ReturnsAsync(cartItems);
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book1);
            _bookRepository.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(book2);

            _shipmentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync(shipment);
            _paymentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Payment>())).ReturnsAsync(payment);
            _orderRepository.Setup(repo => repo.CreateAsync(It.IsAny<Order>())).ReturnsAsync(order);

            _orderItemRepository.Setup(repo => repo.CreateAsync(It.IsAny<OrderItem>())).ReturnsAsync(new OrderItem());
            _bookRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Book>(), It.IsAny<int>())).ReturnsAsync(true);
            _cartItemRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            _mapper.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(new OrderDto { Id = 1 });

            // Act
            var result = await _orderService.CreateAsync(orderDto, shipmentDto, paymentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
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
        {
            // Arrange
            var orderDto = new OrderDto { Id = 1, Status = 2 };
            var order = new Order { Id = 1, Status = 1 };
            _orderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);
            _orderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _orderService.UpdateAsync(orderDto, 1);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateAsync_OrderDoesNotExist_ThrowsException()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 99, Status = 2 };
            _orderRepository.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateAsync(orderDto, 99));
        }

        [Fact]
        public async Task DeleteAsync_OrderExists_DeletesOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            _orderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);
            _orderRepository.Setup(repo => repo.DeleteAsync(orderId)).ReturnsAsync(true);

            // Act
            var result = await _orderService.DeleteAsync(orderId);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task DeleteAsync_OrderDoesNotExist_ThrowsException()
        {
            // Arrange
            var orderId = 99;
            _orderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.DeleteAsync(orderId));
        }

        [Fact]
        public async Task GetAllAsync_ValidRequest_ReturnsPaginatedOrdersWithItems()
        {
            // Arrange
            var orders = new List<Order>
    {
        new Order { Id = 1 },
        new Order { Id = 2 }
    };

            var orderItems1 = new List<OrderItem> { new OrderItem { OrderId = 1 } };
            var orderItems2 = new List<OrderItem> { new OrderItem { OrderId = 2 } };

            _orderRepository.Setup(repo => repo.GetAllAsync(1, 10, null, "Date", true))
                .ReturnsAsync(new PaginatedList<Order>(orders, 2, 1, 10));

            _orderItemRepository.Setup(repo => repo.GetOrderItemsByOrderIdAsync(1))
                .ReturnsAsync(orderItems1);
            _orderItemRepository.Setup(repo => repo.GetOrderItemsByOrderIdAsync(2))
                .ReturnsAsync(orderItems2);

            _mapper.Setup(m => m.Map<OrderDto>(It.IsAny<Order>()))
                .Returns<Order>(o => new OrderDto { Id = o.Id });

            _mapper.Setup(m => m.Map<List<OrderItemDto>>(It.IsAny<List<OrderItem>>()))
                .Returns<List<OrderItem>>(items => items.Select(i => new OrderItemDto { OrderId = i.OrderId }).ToList());

            // Act
            var result = await _orderService.GetAllAsync(1, 10, null, "Date", true);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, o => Assert.NotNull(o.OrderItems));
            Assert.Equal(1, result.Items[0].OrderItems[0].OrderId);
            Assert.Equal(2, result.Items[1].OrderItems[0].OrderId);
        }


        [Fact]
        public async Task GetOrdersByUserIdAsync_ValidUserAndStatus_ReturnsOrdersWithItems()
        {
            // Arrange
            var userId = 1;
            var status = 2;

            var orders = new List<Order>
        {
            new Order { Id = 1, UserId = userId, Status = status },
            new Order { Id = 2, UserId = userId, Status = status }
        };

            var orderItems = new List<OrderItem>
        {
            new OrderItem { Id = 1, OrderId = 1, BookId = 1, Quantity = 2, Price = 100 },
            new OrderItem { Id = 2, OrderId = 2, BookId = 2, Quantity = 1, Price = 200 }
        };

            _orderRepository.Setup(repo => repo.GetOrdersByUserIdAsync(userId, status))
                .ReturnsAsync(orders);

            _orderItemRepository.Setup(repo => repo.GetOrderItemsByOrderIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int orderId) => orderItems.Where(i => i.OrderId == orderId).ToList());

            _mapper.Setup(m => m.Map<OrderDto>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderDto { Id = order.Id, UserId = order.UserId, Status = order.Status });

            _mapper.Setup(m => m.Map<List<OrderItemDto>>(It.IsAny<List<OrderItem>>()))
                .Returns((List<OrderItem> items) => items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    BookId = i.BookId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList());

            // Act
            var result = (await _orderService.GetOrdersByUserIdAsync(userId, status)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.NotNull(order.OrderItems));
            Assert.Equal(1, result[0].OrderItems[0].OrderId);
            Assert.Equal(2, result[1].OrderItems[0].OrderId);
        }


        [Fact]
        public async Task UpdateStatusAsync_OrderExists_UpdatesStatus()
        {
            // Arrange
            var order = new Order { Id = 1, Status = 1 };
            _orderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);
            _orderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _orderService.UpdateStatusAsync(2, 1);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateStatusAsync_StatusIsThree_UpdatesShipmentDate()
        {
            // Arrange
            var order = new Order { Id = 1, Status = 1, ShipmentId = 1 };
            var shipment = new Shipment { Id = 1, DateReceived = DateTime.UtcNow.AddDays(1) };
            _orderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);
            _shipmentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(shipment);
            _orderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Order>(), 1)).ReturnsAsync(true);
            _shipmentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Shipment>(), 1)).ReturnsAsync(true);

            // Act
            var result = await _orderService.UpdateStatusAsync(3, 1);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateStatusAsync_OrderDoesNotExist_ThrowsException()
        {
            // Arrange
            var orderId = 99;
            _orderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateStatusAsync(2, orderId));
        }



    }
}
