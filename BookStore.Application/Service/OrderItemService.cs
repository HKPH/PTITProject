using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class OrderItemService : IOrderItemService 
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public Task<OrderItemDto> CreateAsync(OrderItemDto orderItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItemDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(OrderItemDto orderItemDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
