using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync()
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartItemDto> GetByIdAsync(int id)
        {
            var CartItem = await _cartItemRepository.GetByIdAsync(id);
            if (CartItem == null)
            {
                throw new Exception("CartItem not found.");
            }
            return _mapper.Map<CartItemDto>(CartItem);
        }

        public async Task<CartItemDto> CreateAsync(CartItemDto cartItemDto)
        {
            var itemExists = await _cartItemRepository.CartItemExistsAsync(cartItemDto.CartId, cartItemDto.BookId);
            
            if (itemExists)
            {
                var cartItemsByCart = await _cartItemRepository.GetCartItemsByCartIdAsync(cartItemDto.CartId);
                var cartItem = cartItemsByCart.FirstOrDefault(ci => ci.BookId == cartItemDto.BookId);

                if (cartItem != null)
                {
                    cartItem.Quantity += 1;

                    var checkUpdatedCartItem = await _cartItemRepository.UpdateAsync(cartItem, cartItem.Id);
                    if (checkUpdatedCartItem == false)
                    {
                        throw new Exception("Failed to update CartItem.");
                    }

                    var updatedCartItem = await _cartItemRepository.GetByIdAsync(cartItem.Id);
                    return _mapper.Map<CartItemDto>(updatedCartItem);
                }
            }
            else
            {
                var cartItem = _mapper.Map<CartItem>(cartItemDto);
                var createdCartItem = await _cartItemRepository.CreateAsync(cartItem);
                if (createdCartItem == null)
                {
                    throw new Exception("Failed to create CartItem.");
                }
                return _mapper.Map<CartItemDto>(createdCartItem);
            }

            return null;
        }


        public async Task<bool> UpdateAsync(CartItemDto cartItemDto, int id)
        {
            var checkCartItem = await _cartItemRepository.GetByIdAsync(id);
            if (checkCartItem == null)
            {
                throw new Exception("CartItem not found.");
            }

            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            return await _cartItemRepository.UpdateAsync(cartItem, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkCartItem = await _cartItemRepository.GetByIdAsync(id);
            if (checkCartItem == null)
            {
                throw new Exception("CartItem not found.");
            }

            return await _cartItemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
            if (cartItems == null)
            {
                throw new Exception("CartItems not found.");
            }
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }
    }
}
