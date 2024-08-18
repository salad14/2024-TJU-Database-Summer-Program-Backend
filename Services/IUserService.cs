using VenueBookingSystem.Models;  // 引入 UserDto 和 LoginDto 所在的命名空间

namespace VenueBookingSystem.Services
{
    // 定义 IUserService 接口
    public interface IUserService
    {
        // 用户注册的方法签名
        void Register(UserDto userDto);

        // 用户认证的方法签名
        string Authenticate(LoginDto loginDto);
    }
}
