using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var createdUser = await _userRepository.CreateAsync(user);
            if (createdUser == null)
            {
                throw new Exception("Failed to create User.");
            }
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<bool> UpdateAsync(UserDto userDto, int id)
        {
            var checkUser = await _userRepository.GetByIdAsync(id);
            if (checkUser == null)
            {
                throw new Exception("User not found.");
            }

            var user = _mapper.Map<User>(userDto);
            return await _userRepository.UpdateAsync(user, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkUser = await _userRepository.GetByIdAsync(id);
            if (checkUser == null)
            {
                throw new Exception("User not found.");
            }

            return await _userRepository.DeleteAsync(id);
        }
    }
}
