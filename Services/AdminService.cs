using Microsoft.EntityFrameworkCore;
using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<AdminNotification> _adminNotificationRepository;
        private readonly ApplicationDbContext _context;

        public AdminService(IRepository<Admin> adminRepository, IRepository<Announcement> announcementRepository,IRepository<AdminNotification> adminNotificationRepository,ApplicationDbContext context)
        {
            _adminRepository = adminRepository;
            _announcementRepository = announcementRepository;
            _adminNotificationRepository = adminNotificationRepository;
            _context = context;
        }

        public Admin GetAdminById(string adminId)
        {
            return _adminRepository.Find(a => a.AdminId == adminId).FirstOrDefault();
        }

        public void CreateAdmin(Admin admin)
        {
            _adminRepository.Add(admin);
        }

        public IEnumerable<object> GetPublicNoticeData(string adminId)
        {
            // 首先验证adminId是否存在
            var admin = _adminRepository
            .Find(a => a.AdminId == adminId)
            .Select(a => new AdminRequestDto
            {
                AdminId = a.AdminId,
                ContactNumber = a.ContactNumber,
                Password = a.Password
            });

            if (admin == null)
            {
                throw new ArgumentException("无效的管理员ID");
            }

            // 从 AdminNotifications 表中筛选与 AdminId 相关的通知
            var notifications = _context.AdminNotifications
            .FromSqlRaw("SELECT * FROM \"AdminNotifications\" WHERE \"AdminId\" = {0}", adminId)
            .Select(n => new AdminNotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationType = n.NotificationType,
                Title = n.Title,
                Content = n.Content,
                NotificationTime = n.NotificationTime
            })
            .ToList();

            if (notifications == null)
            {
                return Enumerable.Empty<object>(); // 如果没有找到通知，返回一个空的集合
            }

            return notifications;
        }


        public RegisterResult RegisterAdmin(AdminDto adminDto, List<string> manageVenues)
        {
            try
            {
                // 检查管理员是否已存在
                if (_adminRepository.Find(a => a.ContactNumber == adminDto.ContactNumber).Any())
                {
                    return new RegisterResult
                    {
                        State = 0,
                        AdminId = string.Empty,
                        Info = "管理员已存在"
                    };
                }

                // 生成唯一的管理员ID
                string adminId = GenerateUniqueAdminId();

                // 创建管理员实体
                var admin = new Admin
                {
                    AdminId = adminId,
                    RealName = adminDto.RealName,
                    Password = adminDto.Password,
                    ContactNumber = adminDto.ContactNumber,
                    AdminType = adminDto.AdminType
                };

                // 添加管理员到数据库
                _adminRepository.Add(admin);

                // 如果 manageVenues 不为空，则为每个场地分配管理员
                if (manageVenues != null && manageVenues.Any())
                {
                    foreach (var venueId in manageVenues)
                    {
                        var manageVenue = new VenueManagement
                        {
                            AdminId = adminId,
                            VenueId = venueId
                        };
                        _context.VenueManagements.Add(manageVenue);
                    }
                }

                // 保存更改到数据库
                _context.SaveChanges();

                return new RegisterResult
                {
                    State = 1,
                    AdminId = adminId,
                    Info = "注册成功"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResult
                {
                    State = 0,
                    AdminId = string.Empty,
                    Info = $"注册失败: {ex.Message}"
                };
            }
        }


        // 生成唯一的管理员ID
        private string GenerateUniqueAdminId()
        {
            var random = new Random();
            string adminId;

            do
            {
                adminId = random.Next(100000, 999999).ToString();
            } while (_adminRepository.Find(a => a.AdminId == adminId).Any()); // 确保ID唯一性

            return adminId;
        }


        // 其他方法的实现...
    }
}
