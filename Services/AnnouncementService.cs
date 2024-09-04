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

        private readonly IRepository<Venue> _venueRepository;

        // 构造函数，注入存储库
        public AnnouncementService(IRepository<Announcement> announcementRepository, IRepository<Admin> adminRepository, IRepository<Venue> venueRepository)  // 新增 userRepository 参数
        {
            _adminRepository = adminRepository;
            _announcementRepository = announcementRepository;
            _venueRepository = venueRepository;
        }

        // 发布公告
        public void PublishAnnouncement(AnnouncementDto announcementDto)
        {
            var admin = _adminRepository.Find(a => a.AdminId == announcementDto.AdminId).FirstOrDefault();
<<<<<<< HEAD
            
=======

>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
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

        public AnnouncementVenueDto GetAllAnnouncementsById(string Id)
        {
            var announcements = _announcementRepository.Find(x => x.AnnouncementId == Id).FirstOrDefault();

            List<Venue> Venues = new List<Venue>();
            if (announcements != null && announcements.VenueAnnouncements != null && announcements.VenueAnnouncements.Count > 0)
            {
                foreach (var item in announcements.VenueAnnouncements)
                {
                    var venue = _venueRepository.Find(x => x.VenueId == item.VenueId).FirstOrDefault();
                    if (venue != null)
                    {
                        Venues.Add(venue);
                    }
                }
                return new AnnouncementVenueDto
                {
                    Title = announcements.Title,
                    Content = announcements.Content,
                    PublishDate = announcements.PublishedDate.ToString(),
                    AdminId = announcements.AdminId,
                    Venues = Venues
                };
            }
            else
            {
                return new AnnouncementVenueDto
                {
                    Title = "",
                    Content = "",
                    PublishDate = "",
                    AdminId = "",
                    Venues = Venues
                };
            }
<<<<<<< HEAD
=======



>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        }
    }
}
