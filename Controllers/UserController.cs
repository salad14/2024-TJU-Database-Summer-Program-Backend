using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto; // 确保使用了正确的命名空间
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // 构造函数，注入用户服务
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 注册用户
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            try
            {
                var result = _userService.Register(userDto);

                if (result.State == 1)
                {
                    return Ok(new { message = "注册成功", userId = result.UserId });
                }
                else
                {
                    return BadRequest(new { message = "注册失败", error = result.Info });
                }
            }
            catch (Exception ex)
            {
                // 处理注册时的任何异常
                return BadRequest(new { message = "注册失败", error = ex.Message });
            }
        }

        // 用户登录
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // 根据用户名或用户ID登录
                LoginResult loginResult;

                if (!string.IsNullOrEmpty(loginDto.Username))
                {
                    loginResult = _userService.AuthenticateByUsername(loginDto.Username, loginDto.Password);
                }
                else
                {
                    loginResult = _userService.AuthenticateByUserId(loginDto.UserId, loginDto.Password);
                }

                if (loginResult.State == 1)
                {
                    var token = _userService.GenerateJwtToken(loginResult.UserId, loginResult.UserName, loginResult.IsAdmin);
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized(new { message = loginResult.Info });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "无效的凭据" });
            }
            catch (Exception ex)
            {
                // 处理其他可能的异常
                return BadRequest(new { message = "登录失败", error = ex.Message });
            }
        }

        // 其他用户相关操作...
    }
}
