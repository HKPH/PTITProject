using Microsoft.AspNetCore.Mvc;
using BookStore.Service.Interface;
using BookStore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Helpers;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST: api/account/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccountAndUser([FromBody] AccountAndUserDto request)
        {
            try
            {
                var (createdAccount, createdUser) = await _accountService.CreateAccountAndUserAsync(request.Account, request.User);
                return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, new { createdAccount, createdUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // GET: api/account
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 10, string? searchUsername = null)
        {
            var accounts = await _accountService.GetAllAsync(page, pageSize, searchUsername);
            return Ok(accounts);
        }

        // GET: api/account/{id}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDto accountDto)
        {
            try
            {
                var updated = await _accountService.UpdateAsync(accountDto, id);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _accountService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var (token, userId, refreshToken) = await _accountService.LoginAsync(loginDto.Username, loginDto.Password);
                if (token == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }
                return Ok(new { token, refreshToken, userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/account/{id}/changeActive
        [HttpPut("{id}/changeActive")]
        public async Task<IActionResult> ChangeActive(int id)
        {
            try
            {
                var result = await _accountService.ChangeActiveAsync(id);
                if (result)
                {
                    return Ok(new { message = "Tài khoản đã được cập nhật trạng thái." });
                }
                return NotFound(new { message = "Tài khoản không tồn tại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/account/{id}/resetPassword
        [HttpPut("{id}/resetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(id);
                if (result)
                {
                    return Ok(new { message = "Mật khẩu đã được reset." });
                }
                return NotFound(new { message = "Tài khoản không tồn tại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/account/{id}/changePassword
        [HttpPut("{id}/changePassword")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var result = await _accountService.ChangePasswordAsync(changePasswordDto.OldPassword, id, changePasswordDto.NewPassword);

                if (result)
                {
                    return Ok(new { message = "Mật khẩu đã được thay đổi thành công." });
                }

                return BadRequest(new { message = "Mật khẩu cũ không đúng hoặc tài khoản không tồn tại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/account/refresh

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var newAccessToken = await _accountService.RefreshAccessTokenAsync(refreshToken);
            if (newAccessToken == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(new { token = newAccessToken });
        }



    }
}
