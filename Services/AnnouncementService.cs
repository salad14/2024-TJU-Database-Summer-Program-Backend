using System.Collections.Generic;
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository<Announcement> _announcementRepository;

        // 构造函数，注入存储库
        public AnnouncementService(IRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        // 发布公告
        public void PublishAnnouncement(AnnouncementDto announcementDto)
        {
            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                PublishedDate = DateTime.Now
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
