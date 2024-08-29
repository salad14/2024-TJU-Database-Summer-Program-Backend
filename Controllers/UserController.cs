using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using VenueBookingSystem.Data;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRepository<User> _userRepository;

        // 构造函数，注入用户服务和用户存储库
        public UserController(IUserService userService, IRepository<User> userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }
        
        // 查找某一用户的所有信息
        [HttpGet("{userId}/info")]
        public IActionResult GetUserInfo(string userId)
        {
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { message = "用户未找到" });
            }

            return Ok(new 
            {
                UserId = user.UserId,
                Username = user.Username,
                ContactNumber = user.ContactNumber,
                UserType = user.UserType,
                ReservationPermission = user.ReservationPermission,
                ViolationCount = user.ViolationCount,
                RegistrationDate = user.RegistrationDate
            });
        }

        // 查找某一用户相关的团体信息
        [HttpGet("{userId}/groups")]
        public IActionResult GetUserGroupInfo(string userId)
        {
            var userGroupInfo = _userService.GetUserGroupInfo(userId);
            if (userGroupInfo == null)
            {
                return NotFound(new { message = "未找到用户的团体信息" });
            }

            return Ok(userGroupInfo);
        }
        // 注册用户
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            try
            {
                // 为 ReservationPermission 和 IsVip 设置默认值
                userDto.ReservationPermission ??= "n";
                userDto.IsVip ??= "n";

                // 调用服务层进行注册
                var result = _userService.Register(userDto);

                return Ok(result);  // 直接返回 RegisterResult 对象
            }
            catch (Exception ex)
            {
                // 处理注册时的任何异常，返回失败的 RegisterResult
                return BadRequest(new RegisterResult 
                { 
                    State = 0, 
                    UserId = null, 
                    Info = $"注册失败: {ex.Message}" 
                });
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

        // 查找某一用户的通知
        [HttpGet("{userId}/notifications")]
        public IActionResult GetUserNotifications(string userId)
        {
            var notifications = _userService.GetUserNotifications(userId);

            if (notifications == null || !notifications.Any())
            {
                // 如果没有找到通知，返回空的通知列表
                return Ok(new List<UserNotificationDto>());
            }
            
            return Ok(notifications);
        }

        // 其他用户相关操作...
    }
}
