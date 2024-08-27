using VenueBookingSystem.Models;
using VenueBookingSystem.Data;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VenueBookingSystem.Dto;


namespace VenueBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Admin> _adminRepository;
        private readonly IConfiguration _configuration;


        // 构造函数，注入存储库和配置
        public UserService(IRepository<User> userRepository, IRepository<Admin> adminRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _configuration = configuration;
        }

        // 注册用户
        public RegisterResult Register(UserDto userDto)
        {
            // 确保用户名、真实姓名和联系电话唯一
            if (_userRepository.Find(u => u.Username == userDto.Username).Any() ||
                _userRepository.Find(u => u.RealName == userDto.RealName).Any() ||
                _userRepository.Find(u => u.ContactNumber == userDto.ContactNumber).Any())
            {
                return new RegisterResult
                {
                    State = 0,
                    Info = "用户名、真实姓名或联系电话已被注册"
                };
            }

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(), // 分配唯一的用户ID
                Username = userDto.Username,
                RealName = userDto.RealName,
                Password = userDto.Password, // 已加密密码，直接存储
                ContactNumber = userDto.ContactNumber,
                RegistrationDate = DateTime.Now,
                UserType = "普通用户" // 默认类型
            };

            _userRepository.Add(user);

            return new RegisterResult
            {
                State = 1,
                UserId = user.UserId,
                Info = ""
            };
        }

        public LoginResult AuthenticateByUsername(string username, string password)
        {
            var user = _userRepository.Find(u => u.Username == username).FirstOrDefault();

            if (user != null && user.Password == password)
            {
                return new LoginResult 
                { 
                    State = 1, 
                    UserId = user.UserId, 
                    UserName = user.Username, 
                    UserType = user.UserType,
                    Info = "" 
                };
            }
            else
            {
                return new LoginResult 
                { 
                    State = 0, 
                    UserId = string.Empty, 
                    UserName = string.Empty, 
                    UserType = string.Empty, 
                    Info = "账号或密码错误" 
                };
            }
        }

        public LoginResult AuthenticateByUserId(string userId, string password)
        {
            var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();

            if (user != null && user.Password == password)
            {
                return new LoginResult 
                { 
                    State = 1, 
                    UserId = user.UserId, 
                    UserName = user.Username, 
                    UserType = user.UserType,
                    Info = "" 
                };
            }
            else
            {
                return new LoginResult 
                { 
                    State = 0, 
                    UserId = string.Empty, 
                    UserName = string.Empty, 
                    UserType = string.Empty, 
                    Info = "账号或密码错误" 
                };
            }
        }



        // 密码哈希处理
        private string HashPassword(string password)
        {
            using (var rng = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
            {
                byte[] salt = rng.Salt;
                byte[] key = rng.GetBytes(32);
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
            }
        }

        // 验证密码
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                throw new FormatException("存储的哈希格式无效");

            var salt = Convert.FromBase64String(parts[0]);
            var storedKey = Convert.FromBase64String(parts[1]);

            using (var rng = new Rfc2898DeriveBytes(inputPassword, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] keyToCheck = rng.GetBytes(32);
                return keyToCheck.SequenceEqual(storedKey);
            }
        }

        // 生成JWT令牌
        public string GenerateJwtToken(string userId, string userName, string userType)
        {
            // 根据 userType 判断角色
            string role;
            switch (userType.ToLower())
            {
                case "system":
                case "venue":
                case "device":
                case "venue-device":
                    role = "管理员";
                    break;
                case "normal":
                    role = "普通用户";
                    break;
                default:
                    throw new ArgumentException("无效的用户类型", nameof(userType));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role) // 将用户角色添加为声明
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        // 其他用户服务逻辑...
    }
}