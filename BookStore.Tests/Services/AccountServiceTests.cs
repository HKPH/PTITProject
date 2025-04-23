using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Helpers;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Service;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;

namespace BookStore.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ICartRepository> _cartRepoMock = new();
        private readonly Mock<IShippingAddressRepository> _shippingAddressRepoMock = new();
        private readonly Mock<IJwtHelper> _jwtHelperMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService(
                _accountRepoMock.Object,
                _cartRepoMock.Object,
                _userRepoMock.Object,
                _shippingAddressRepoMock.Object,
                _jwtHelperMock.Object,
                _mapperMock.Object
            );
        }

        private (AccountDto accountDto, UserDto userDto, Account mappedAccount, User mappedUser, User createdUser) PrepareTestData()
        {
            var accountDto = new AccountDto
            {
                Username = "testuser",
                Password = "testpass",
                Email = "test@example.com"
            };

            var userDto = new UserDto
            {
                Name = "Test User",
                Phone = "123456789",
                Address = "123 Street",
                Dob = new DateTime(2000, 1, 1),
                Gender = 0
            };

            var mappedAccount = new Account
            {
                Username = accountDto.Username,
                Password = accountDto.Password,
                Email = accountDto.Email
            };

            var mappedUser = new User
            {
                Name = userDto.Name,
                Phone = userDto.Phone,
                Address = userDto.Address,
                Dob = userDto.Dob,
                Gender = userDto.Gender
            };

            var createdUser = new User
            {
                Id = 1,
                Name = userDto.Name,
                Phone = userDto.Phone,
                Address = userDto.Address,
                Dob = userDto.Dob,
                Gender = userDto.Gender,
                Email = accountDto.Email
            };

            return (accountDto, userDto, mappedAccount, mappedUser, createdUser);
        }

        [Fact]
        public async Task CreateAccountAndUserAsync_ValidInput_ReturnsAccountAndUserDto()
        {
            // Arrange
            var (accountDto, userDto, mappedAccount, mappedUser, createdUser) = PrepareTestData();

            var createdAccount = new Account { Id = 2, Username = accountDto.Username, UserId = createdUser.Id };

            _mapperMock.Setup(m => m.Map<Account>(accountDto)).Returns(mappedAccount);
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(mappedUser);
            _accountRepoMock.Setup(r => r.CheckUsernameExistsAsync(accountDto.Username)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
            _accountRepoMock.Setup(r => r.CreateAsync(It.IsAny<Account>())).ReturnsAsync(createdAccount);
            _shippingAddressRepoMock.Setup(r => r.CreateAsync(It.IsAny<ShippingAddress>())).ReturnsAsync(new ShippingAddress());
            _cartRepoMock.Setup(r => r.CreateAsync(It.IsAny<Cart>())).ReturnsAsync(new Cart());
            _mapperMock.Setup(m => m.Map<AccountDto>(It.IsAny<Account>())).Returns(new AccountDto { Id = 2, Username = accountDto.Username });
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Id = 1, Name = userDto.Name });

            // Act
            var result = await _accountService.CreateAccountAndUserAsync(accountDto, userDto);

            // Assert
            Assert.Equal(accountDto.Username, result.accountDto.Username);
            Assert.Equal(userDto.Name, result.userDto.Name);
        }

        [Fact]
        public async Task CreateAccountAndUserAsync_UsernameExists_ThrowsException()
        {
            // Arrange
            var accountDto = new AccountDto { Username = "existing" };
            var userDto = new UserDto();

            _mapperMock.Setup(m => m.Map<Account>(accountDto)).Returns(new Account { Username = accountDto.Username });
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(new User());
            _accountRepoMock.Setup(r => r.CheckUsernameExistsAsync(accountDto.Username)).ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.CreateAccountAndUserAsync(accountDto, userDto));
            Assert.Equal("Username already exists.", ex.Message);
        }

        [Fact]
        public async Task CreateAccountAndUserAsync_CreateUserFails_ThrowsException()
        {
            // Arrange
            var (accountDto, userDto, mappedAccount, mappedUser, _) = PrepareTestData();

            _mapperMock.Setup(m => m.Map<Account>(accountDto)).Returns(mappedAccount);
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(mappedUser);
            _accountRepoMock.Setup(r => r.CheckUsernameExistsAsync(accountDto.Username)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync((User?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.CreateAccountAndUserAsync(accountDto, userDto));
            Assert.Equal("Failed to create", ex.Message);
        }

        [Fact]
        public async Task CreateAccountAndUserAsync_CreateAccountFails_ThrowsException()
        {
            // Arrange
            var (accountDto, userDto, mappedAccount, mappedUser, createdUser) = PrepareTestData();

            _mapperMock.Setup(m => m.Map<Account>(accountDto)).Returns(mappedAccount);
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(mappedUser);
            _accountRepoMock.Setup(r => r.CheckUsernameExistsAsync(accountDto.Username)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
            _accountRepoMock.Setup(r => r.CreateAsync(It.IsAny<Account>())).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.CreateAccountAndUserAsync(accountDto, userDto));
            Assert.Equal("Failed to create", ex.Message);
        }

        [Fact]
        public async Task CreateAccountAndUserAsync_CreateShippingAddressFails_ThrowsException()
        {
            // Arrange
            var (accountDto, userDto, mappedAccount, mappedUser, createdUser) = PrepareTestData();

            _mapperMock.Setup(m => m.Map<Account>(accountDto)).Returns(mappedAccount);
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(mappedUser);
            _accountRepoMock.Setup(r => r.CheckUsernameExistsAsync(accountDto.Username)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
            _accountRepoMock.Setup(r => r.CreateAsync(It.IsAny<Account>())).ReturnsAsync(new Account { Id = 2 });
            _shippingAddressRepoMock.Setup(r => r.CreateAsync(It.IsAny<ShippingAddress>())).ReturnsAsync((ShippingAddress?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.CreateAccountAndUserAsync(accountDto, userDto));
            Assert.Equal("Failed to create", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountDto = new AccountDto { Id = 1, Username = "testuser", Password = "newpassword" };
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountDto.Id)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.UpdateAsync(accountDto, accountDto.Id));
            Assert.Equal("Account not exist", ex.Message);
        }

        [Fact]
        public async Task ResetPasswordAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.ResetPasswordAsync(accountId));
            Assert.Equal("Account not exist", ex.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            var oldPassword = "oldpassword";
            var newPassword = "newpassword";
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.ChangePasswordAsync(oldPassword, accountId, newPassword));
            Assert.Equal("Account not exist", ex.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_OldPasswordDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            var accountId = 1;
            var oldPassword = "oldpassword";
            var newPassword = "newpassword";
            var account = new Account { Id = 1, Username = "testuser", Password = "differentoldpassword" };

            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _accountService.ChangePasswordAsync(oldPassword, accountId, newPassword);

            // Assert
            Assert.False(result);  
        }

        [Fact]
        public async Task ChangeActiveAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.ChangeActiveAsync(accountId));
            Assert.Equal("Account not exist", ex.Message);
        }

        [Fact]
        public async Task ChangeActiveAsync_Success_ChangesActiveStatus()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = 1, Username = "testuser", Role = 1 };

            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
            _accountRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Account>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _accountService.ChangeActiveAsync(accountId);

            // Assert
            Assert.True(result);
            Assert.Equal(0, account.Role);
        }


        [Fact]
        public async Task GetByIdAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.GetByIdAsync(accountId));
            Assert.Equal("Account not found.", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Success_ReturnsAccountDto()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, Username = "testuser" };

            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
            _mapperMock.Setup(m => m.Map<AccountDto>(account)).Returns(new AccountDto { Id = accountId, Username = "testuser" });

            // Act
            var result = await _accountService.GetByIdAsync(accountId);

            // Assert
            Assert.Equal(accountId, result.Id);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task DeleteAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _accountService.DeleteAsync(accountId));
            Assert.Equal("Account not found.", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_Success_ReturnsTrue()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, Username = "testuser" };

            _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
            _accountRepoMock.Setup(r => r.DeleteAsync(accountId)).ReturnsAsync(true);

            // Act
            var result = await _accountService.DeleteAsync(accountId);

            // Assert
            Assert.True(result);
        }


    }
}
