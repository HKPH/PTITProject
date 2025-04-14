using BookStore.Application.Helpers;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using BookStore.Application.Service.Interface;
using AutoMapper;
using System.Diagnostics;
using BookStore.Application.Dtos;

namespace BookStore.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShippingAddressRepository _shippingAddressRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository repository, ICartRepository cartRepository, IUserRepository userRepository,
            IShippingAddressRepository shippingAddressRepository, IJwtHelper jwtHelper, IMapper mapper)
        {
            _accountRepository = repository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _shippingAddressRepository = shippingAddressRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;

        }
        public async Task<(AccountDto accountDto, UserDto userDto)> CreateAccountAndUserAsync(AccountDto accountDto, UserDto userDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            var user = _mapper.Map<User>(userDto);

            if (await _accountRepository.CheckUsernameExistsAsync(account.Username))
            {
                throw new Exception("Username already exists.");
            }
            var newUser = new User
            {
                Name = user.Name,
                Phone = user.Phone,
                Email = account.Email,
                Address = user.Address,
                Dob = user.Dob,
                Gender = user.Gender,
                Active = true
            };
            var createdUser = await _userRepository.CreateAsync(newUser);
            if (createdUser == null)
            {
                throw new Exception("Failed to create");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.Password);
            var newAccount = new Account
            {
                Username = account.Username,
                Password = hashedPassword,
                CreateDate = DateTime.Now,
                UserId = newUser.Id,
                Role = 1,
                Active = true
            };
            var createdAccount = await _accountRepository.CreateAsync(newAccount);
            if (createdAccount == null)
            {
                throw new Exception("Failed to create");
            }

            var newCart = new Cart
            {
                UserId = createdUser.Id
            };

            var newShippingAddress = new ShippingAddress
            {
                Address = newUser.Address,
                Note = "",
                CustomerNumber = newUser.Phone,
                UserId = createdUser.Id
            };
            var createdShippingAddress = await _shippingAddressRepository.CreateAsync(newShippingAddress);
            if (createdShippingAddress == null)
            {
                throw new Exception("Failed to create");
            }


            await _cartRepository.CreateAsync(newCart);
            return (_mapper.Map<AccountDto>(createdAccount), _mapper.Map<UserDto>(createdUser));

        }
        public async Task<bool> UpdateAsync(AccountDto accountDto, int id)
        {
            var account = _mapper.Map<Account>(accountDto);

            var accountExit = await _accountRepository.GetByIdAsync(id);
            if (accountExit == null)
            {
                throw new Exception("Account not exit");
            }
            if (!string.IsNullOrEmpty(account.Password))
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }

            return await _accountRepository.UpdateAsync(account, id);
        }
        public async Task<bool> ResetPasswordAsync(int id)
        {
            var accountExit = await _accountRepository.GetByIdAsync(id);
            if (accountExit == null)
            {
                throw new Exception("Account not exit");
            }
            accountExit.Password = BCrypt.Net.BCrypt.HashPassword("1");

            return await _accountRepository.UpdateAsync(accountExit, id);
        }
        public async Task<bool> ChangePasswordAsync(string oldPassword, int id, string newPassword)
        {
            var accountExit = await _accountRepository.GetByIdAsync(id);
            if (accountExit == null)
            {
                throw new Exception("Account not exit");
            }
            if (accountExit.Password == BCrypt.Net.BCrypt.HashPassword(oldPassword))
            {
                accountExit.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }

            return await _accountRepository.UpdateAsync(accountExit, id);
        }
        public async Task<bool> ChangeActiveAsync(int id)
        {
            var accountExit = await _accountRepository.GetByIdAsync(id);
            if (accountExit == null)
            {
                throw new Exception("Account not exit");
            }
            if (accountExit.Role == 0)
            {
                accountExit.Role = 1;
            }
            else if (accountExit.Role == 1)
            {
                accountExit.Role = 0;
            }

            return await _accountRepository.UpdateAsync(accountExit, id);
        }
        public async Task<(string token, int userId, string refreshToken)> LoginAsync(string username, string password)
        {
            var account = await _accountRepository.GetByUsernameAsync(username);

            if (account == null || !VerifyPassword(password, account.Password))
            {
                throw new Exception("Invalid username or password.");
            }
            if (account.Active != true)
            {
                throw new Exception("Account is not active.");
            }
            var token = _jwtHelper.GenerateJwtToken(account.Id, account.Role.GetValueOrDefault());
            var refreshToken = await _jwtHelper.GenerateAndCacheRefreshTokenAsync(account.Id);
            return (token, account.UserId.GetValueOrDefault(), refreshToken);
        }
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }


        public async Task<PaginatedList<AccountDto>> GetAllAsync(int page, int pageSize, string? searchUsername = null)
        {
            var pageList = await _accountRepository.GetAllAsync(page, pageSize, searchUsername);
            var accountDtos = _mapper.Map<List<AccountDto>>(pageList.Items);
            return new PaginatedList<AccountDto>(
                   accountDtos, pageList.TotalCount, pageList.PageIndex, pageList.PageSize);
        }

        public async Task<AccountDto> GetByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }
            return _mapper.Map<AccountDto>(account);
        }

        public Task<Account> CreateAsync(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(id);
            if (existingAccount == null)
            {
                throw new Exception("Account not found.");
            }
            return await _accountRepository.DeleteAsync(id);
        }

        public async Task<string?> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenData = await _jwtHelper.ValidateRefreshTokenAsync(refreshToken);
            if (tokenData is null) return null;

            var account = await GetByIdAsync(tokenData.UserId);
            if (account == null) return null;

            return _jwtHelper.GenerateJwtToken(account.Id, account.Role);
        }

    }
}
