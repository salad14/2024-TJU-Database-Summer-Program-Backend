using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto; // 确保使用了正确的命名空间
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("_allowSpecificOrigins")]
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
                return BadRequest(new { message = "注册失败1234", error = ex.Message });
            }
        }

        // 用户登录
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
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
                    var token = _userService.GenerateJwtToken(loginResult.UserId, loginResult.UserName, loginResult.UserType);
                    return Ok(new 
                    { 
                        status = 1,  // 成功状态
                        token, 
                        loginResult  // 返回所有 loginResult 信息
                    });
                }
                else
                {
                    // 根据不同的状态码返回更详细的错误信息
                    string errorMessage = loginResult.Info.Contains("账号或密码错误")
                        ? "账号或密码错误"
                        : "登录失败，" + loginResult.Info;

                    return Unauthorized(new 
                    { 
                        status = 0,  // 失败状态
                        message = errorMessage 
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new 
                { 
                    status = 0,  // 失败状态
                    message = "无效的凭据" 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new 
                { 
                    status = 0,  // 失败状态
                    message = "登录失败", 
                    error = ex.Message 
                });
            }
        }




        // 其他用户相关操作...
    }
}
