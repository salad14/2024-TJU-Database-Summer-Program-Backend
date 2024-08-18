using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        // 构造函数，注入用户服务
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 注册用户
        [HttpPost]
        public IActionResult Register(UserDto userDto)
        {
            _userService.Register(userDto);
            return Ok("注册成功");
        }

        // 用户登录
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            var token = _userService.Authenticate(loginDto);
            return Ok(token);
        }

        // 其他用户相关操作...
    }
}
