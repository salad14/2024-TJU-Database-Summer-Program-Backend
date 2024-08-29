using Microsoft.EntityFrameworkCore;
using VenueBookingSystem.Data;
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
                Username = a.Username,
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



        // 其他方法的实现...
    }
}
