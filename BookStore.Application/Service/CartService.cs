using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            var carts = await _cartRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CartDto>>(carts);
        }

        public async Task<CartDto> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> CreateAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            var createdCart = await _cartRepository.CreateAsync(cart);
            if (createdCart == null)
            {
                throw new Exception("Failed to create cart.");
            }
            return _mapper.Map<CartDto>(createdCart);
        }

        public async Task<bool> UpdateAsync(CartDto cartDto, int id)
        {
            var checkCart = await _cartRepository.GetByIdAsync(id);
            if (checkCart == null)
            {
                throw new Exception("Cart not found.");
            }

            var cart = _mapper.Map<Cart>(cartDto);
            return await _cartRepository.UpdateAsync(cart, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkCart = await _cartRepository.GetByIdAsync(id);
            if (checkCart == null)
            {
                throw new Exception("Cart not found.");
            }

            return await _cartRepository.DeleteAsync(id);
        }

        public async Task<CartDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            var cartDto = _mapper.Map<CartDto>(cart);
            return cartDto;
        }
    }
}
