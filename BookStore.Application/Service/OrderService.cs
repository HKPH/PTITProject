﻿using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,
            IShipmentRepository shipmentRepository, IPaymentRepository paymentRepository, 
            ICartItemRepository cartItemRepository, ICartRepository cartRepository,
            IBookRepository bookRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _shipmentRepository = shipmentRepository;
            _paymentRepository = paymentRepository;
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, ShipmentDto shipmentDto, PaymentDto paymentDto)
        {
           
            var cart = await _cartRepository.GetCartByUserIdAsync(orderDto.UserId);
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
            if(cartItems.Count() == 0)
            {
                throw new Exception("Cart is empty.");
            }

            decimal? totalAmount = 0;
            foreach (var cartItem in cartItems)
            {
               
                var book = await _bookRepository.GetByIdAsync(cartItem.BookId);

                if (book != null && cartItem.Quantity.HasValue)
                {
                    totalAmount += book.Price * cartItem.Quantity.Value;
                }

            }

            var shipment = new Shipment
            {
                ShippingAddressId = shipmentDto.ShippingAddressId,
                Fee = 0, 
                DateReceived = DateTime.UtcNow.AddDays(3)
            };
            var createdShipment = await _shipmentRepository.CreateAsync(shipment);
            if (createdShipment == null)
            {
                throw new Exception("Failed to create Shipment.");
            }

            var payment = new Payment
            {
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = totalAmount + shipment.Fee
            };
            var createdPayment = await _paymentRepository.CreateAsync(payment);
            if (createdPayment == null)
            {
                throw new Exception("Failed to create Payment.");
            }

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = 1,
                PaymentId = createdPayment.Id,
                ShipmentId = createdShipment.Id,
                TotalPrice = totalAmount,
            };

            foreach (var cartItem in cartItems)
            {
                var book = await _bookRepository.GetByIdAsync(cartItem.BookId);
                if (book.StockQuantity < cartItem.Quantity)
                {
                    throw new Exception("Not enough stock");
                }
            }
            var createdOrder = await _orderRepository.CreateAsync(order);
            if (createdOrder == null)
            {
                throw new Exception("Failed to create Order.");
            }

            foreach (var cartItem in cartItems)
            {
                var book = await _bookRepository.GetByIdAsync(cartItem.BookId);

                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity,
                    Price = book.Price
                };
                var createdorderItem = await _orderItemRepository.CreateAsync(orderItem);
                if (createdorderItem == null)
                {
                    throw new Exception("Failed to create OrderItem");
                }
                book.StockQuantity -= cartItem.Quantity;
                await _bookRepository.UpdateAsync(book, cartItem.BookId);
                await _cartItemRepository.DeleteAsync(cartItem.Id);
            }

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<bool> UpdateAsync(OrderDto orderDto, int id)
        {
            var checkOrder = await _orderRepository.GetByIdAsync(id);

            if (checkOrder == null)
            {
                throw new Exception("Order not found.");
            }

            var order = _mapper.Map<Order>(orderDto);
            return await _orderRepository.UpdateAsync(order, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkOrder = await _orderRepository.GetByIdAsync(id);
            if (checkOrder == null)
            {
                throw new Exception("Order not found.");
            } 

            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<PaginatedList<OrderDto>> GetAllAsync(int page, int pageSize, string? searchTerm, string sortBy, bool sortDirection)
        {
            var pageList = await _orderRepository.GetAllAsync(page,pageSize, searchTerm, sortBy, sortDirection);   
            var orderDtos = new List<OrderDto>();
            foreach (var order in pageList.Items)
            {
                var orderItems = await _orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);

                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.OrderItems = _mapper.Map<List<OrderItemDto>>(orderItems);

                orderDtos.Add(orderDto);
            }
            return new PaginatedList<OrderDto>(
                            orderDtos, pageList.TotalCount, pageList.PageIndex, pageList.PageSize);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId, int status)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId, status);

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderItems = await _orderItemRepository.GetOrderItemsByOrderIdAsync(order.Id);

                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.OrderItems = _mapper.Map<List<OrderItemDto>>(orderItems);

                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<bool> UpdateStatusAsync(int status, int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                throw new Exception("Order not found.");
            }    
            order.Status = status;
            if(order.Status == 3)
            {
                var shipment = await _shipmentRepository.GetByIdAsync(order.ShipmentId);
                shipment.DateReceived = DateTime.UtcNow;
                await _shipmentRepository.UpdateAsync(shipment, order.ShipmentId);
            }    
            return await _orderRepository.UpdateAsync(order, id);
        }

    }
}
