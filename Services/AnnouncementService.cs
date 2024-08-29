using System.Collections.Generic;
using System.Linq; // 必需的命名空间，用于 LINQ 操作
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<Admin> _adminRepository;

        // 构造函数，注入存储库
        public AnnouncementService(IRepository<Announcement> announcementRepository, IRepository<Admin> adminRepository)  // 新增 userRepository 参数
        {
            _adminRepository = adminRepository;
            _announcementRepository = announcementRepository;
        }

        // 发布公告
        public void PublishAnnouncement(AnnouncementDto announcementDto)
        {
            var admin = _adminRepository.Find(a => a.AdminId == announcementDto.AdminId).FirstOrDefault();
            
            if (admin == null)
            {
                throw new ArgumentException("无效的管理员ID");
            }

            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                PublishedDate = DateTime.Now,
                AdminId = announcementDto.AdminId // 使用 AdminId 关联公告与管理员
            };

            _announcementRepository.Add(announcement);
        }


        // 获取所有公告
        public IEnumerable<AnnouncementDto> GetAllAnnouncements()
        {
            var announcements = _announcementRepository.GetAll();
            return announcements.Select(a => new AnnouncementDto
            {
                Title = a.Title,
                Content = a.Content,
                PublishDate = a.PublishedDate,
                AdminId = a.AdminId
            });
        }
    }
}
