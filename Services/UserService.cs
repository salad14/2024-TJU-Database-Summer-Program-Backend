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
        private readonly IRepository<GroupUser> _groupUserRepository;
        private readonly IRepository<UserNotification> _userNotificationRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Admin> _adminRepository;


        // 构造函数，注入存储库和配置
        public UserService(IRepository<User> userRepository, IRepository<GroupUser> groupUserRepository, IRepository<Group> groupRepository, IRepository<UserNotification> userNotificationRepository, IRepository<Admin> adminRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _userNotificationRepository = userNotificationRepository;
            _configuration = configuration;
            _adminRepository = adminRepository;
        }

        public User GetUserById(string userId)
        {
            return _userRepository.Find(u => u.UserId == userId).FirstOrDefault();
        }

        public UserGroupInfoDto GetUserGroupInfo(string userId)
        {
            var groupUser = _groupUserRepository.Find(gu => gu.UserId == userId).FirstOrDefault();
            if (groupUser == null)
            {
                return null; // 或者返回一个表示没有找到的DTO
            }

            var group = _groupRepository.Find(g => g.GroupId == groupUser.GroupId).FirstOrDefault();
            if (group == null)
            {
                return null; // 或者返回一个表示没有找到的DTO
            }

            return new UserGroupInfoDto
            {
                JoinDate = groupUser.JoinDate,
                RoleInGroup = groupUser.RoleInGroup,
                GroupId = group.GroupId,
                GroupName = group.GroupName
            };
        }

        public IEnumerable<UserNotificationDto> GetUserNotifications(string userId)
        {
            var notifications = _userNotificationRepository.Find(n => n.UserId == userId)
                .Select(n => new UserNotificationDto
                {
                    NotificationId = n.NotificationId,
                    NotificationType = n.NotificationType,
                    Title = n.Title,
                    Content = n.Content,
                    NotificationTime = n.NotificationTime,
                    TargetUser = n.TargetUser,
                    TargetTeam = n.TargetTeam
                }).ToList();

            return notifications;
        }

        // 注册用户
        public RegisterResult Register(UserDto userDto)
        {
            // 确保用户名和联系电话唯一
            if (_userRepository.Find(u => u.Username == userDto.Username).Any())
            {
                return new RegisterResult
                {
                    State = 0,
                    UserId = null,
                    Info = "用户名已被注册"
                };
            }

            if (_userRepository.Find(u => u.ContactNumber == userDto.ContactNumber).Any())
            {
                return new RegisterResult
                {
                    State = 0,
                    UserId = null,
                    Info = "联系电话已被注册"
                };
            }

            // 生成唯一的用户ID
            string userId = GenerateUniqueUserId();

            // 创建用户实体
            var user = new User
            {
                UserId = userId,
                Username = userDto.Username,
                RealName = userDto.RealName,
                Password = userDto.Password, // 已加密密码，直接存储
                ContactNumber = userDto.ContactNumber,
                RegistrationDate = DateTime.Now,
                UserType = "normal", // 默认类型
                ReservationPermission = userDto.ReservationPermission,
                IsVip = userDto.IsVip
            };

            // 添加用户到数据库
            _userRepository.Add(user);

            return new RegisterResult
            {
                State = 1,
                UserId = user.UserId,
                Info = "注册成功"
            };
        }

        // 生成唯一的用户ID
        private string GenerateUniqueUserId()
        {
            var random = new Random();
            string userId;

            do
            {
                userId = random.Next(100000, 999999).ToString();
            } while (_userRepository.Find(u => u.UserId == userId).Any()); // 确保ID唯一性

            return userId;
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

        public LoginResult AuthenticateUser(string mode, string userInfo, string password)
        {
            User? user = null;
            if (mode == "id")
            {
                user = _userRepository.Find(u => u.UserId == userInfo).FirstOrDefault();
            }
            else if (mode == "name")
            {
                user = _userRepository.Find(u => u.Username == userInfo).FirstOrDefault();
            }
            else
            {
                return new LoginResult
                {
                    State = 0,
                    UserId = "",
                    UserName = "",
                    UserType = "",
                    Info = "mode参数无效",
                };
            }
            if (user != null)
            {
                if (user.Password == password)
                {
                    return new LoginResult
                    {
                        State = 1,
                        UserId = user.UserId,
                        UserName = user.Username,
                        UserType = "normal",
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
            Admin? admin = null;
            if (mode == "id")
            {
                admin = _adminRepository.Find(u => u.AdminId == userInfo).FirstOrDefault();
            }
            else if (mode == "name")
            {
                admin = _adminRepository.Find(u => u.RealName == userInfo).FirstOrDefault();
            }
            if (admin != null && admin.Password == password)
            {
                if (admin.AdminType.StartsWith("team/validate"))
                {
                    return new LoginResult
                    {
                        State = 0,
                        UserId = string.Empty,
                        UserName = string.Empty,
                        UserType = string.Empty,
                        Info = "管理员账号审核中"
                    };
                }
                return new LoginResult
                {
                    State = 1,
                    UserId = admin.AdminId,
                    UserName = admin.RealName,
                    UserType = admin.AdminType,
                    Info = ""
                };
            }
            return new LoginResult
            {
                State = 0,
                UserId = string.Empty,
                UserName = string.Empty,
                UserType = string.Empty,
                Info = "账号或密码错误"
            };
        }

        // public LoginResult AuthenticateByUserId(string userId, string password)
        // {
        //     var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();

        //     if (user != null && user.Password == password)
        //     {
        //         return new LoginResult
        //         {
        //             State = 1,
        //             UserId = user.UserId,
        //             UserName = user.Username,
        //             UserType = user.UserType,
        //             Info = ""
        //         };
        //     }
        //     else
        //     {
        //         return new LoginResult
        //         {
        //             State = 0,
        //             UserId = string.Empty,
        //             UserName = string.Empty,
        //             UserType = string.Empty,
        //             Info = "账号或密码错误"
        //         };
        //     }
        // }

        public UpdateResult UpdateUserInfo(string userId, string username, string contactNumber, string realName)
        {
            var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                return new UpdateResult
                {
                    State = 0,
                    Info = "用户未找到"
                };
            }

            // 更新用户信息
            user.Username = username;
            user.ContactNumber = contactNumber;
            user.RealName = realName;

            _userRepository.Update(user);

            return new UpdateResult
            {
                State = 1,
                Info = "用户信息更新成功"
            };
        }

        public UpdateResult UpdateUserPassword(string userId, string newPassword)
        {
            var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                return new UpdateResult
                {
                    State = 0,
                    Info = "用户未找到"
                };
            }

            // 更新用户密码
            user.Password = HashPassword(newPassword);

            _userRepository.Update(user);

            return new UpdateResult
            {
                State = 1,
                Info = "用户密码更新成功"
            };
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
        public UpdateResult DeleteUserNotification(string noticeId)
        {
            var notification = _userNotificationRepository.Find(n => n.NotificationId == noticeId).FirstOrDefault();
            if (notification == null)
            {
                return new UpdateResult
                {
                    State = 0,
                    Info = "通知不存在"
                };
            }
            _userNotificationRepository.Delete(notification);
            return new UpdateResult
            {
                State = 1,
                Info = "删除成功"
            };
        }
    }
}