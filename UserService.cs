using VenueBookingSystem.Models;
using VenueBookingSystem.Data;

namespace VenueBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        // 构造函数，注入存储库
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // 注册用户
        public void Register(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = HashPassword(userDto.Password),
                ContactNumber = userDto.ContactNumber,
                IsVip = userDto.IsVip,
                RegistrationDate = DateTime.Now
            };
            _userRepository.Add(user);
        }

        // 用户认证
        public string Authenticate(LoginDto loginDto)
        {
            // 获取第一个符合条件的用户对象
            var user = _userRepository.Find(u => u.Username == loginDto.Username).FirstOrDefault();
            
            // 如果未找到用户或者密码不匹配，抛出未授权访问异常
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("无效的凭据");
    }

    // 如果验证通过，生成JWT令牌并返回
    return GenerateJwtToken(user);
        }

        // 密码哈希处理
        private string HashPassword(string password)
        {
            // 密码哈希逻辑
            return password; // 示例：仅返回原始密码
        }

        // 验证密码
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // 密码验证逻辑
            return inputPassword == storedHash;
        }

        // 生成JWT令牌
        private string GenerateJwtToken(User user)
        {
            // JWT生成逻辑
            return "token"; // 示例：返回示例令牌
        }

        // 其他用户服务逻辑...
    }
}
