using System.Collections.Generic;
using System.Linq; // 必需的命名空间，用于 LINQ 操作
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<User> _userRepository;  // 新增

        // 构造函数，注入存储库
        public AnnouncementService(IRepository<Announcement> announcementRepository, IRepository<User> userRepository)  // 新增 userRepository 参数
        {
            _announcementRepository = announcementRepository;
            _userRepository = userRepository;  // 初始化 _userRepository
        }

        // 发布公告
        public void PublishAnnouncement(AnnouncementDto announcementDto)
        {
            var user = _userRepository.Find(u => u.UserId == announcementDto.UserId).FirstOrDefault();
            
            if (user == null)
            {
                throw new ArgumentException("无效的用户ID");
            }

            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                PublishedDate = DateTime.Now,
                User = user  // 设置必需的 User 属性
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
                PublishDate = a.PublishedDate
            });
        }
    }
}
