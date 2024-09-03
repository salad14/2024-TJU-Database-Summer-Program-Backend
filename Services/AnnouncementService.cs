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

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Venue> _venueRepository;

        // 构造函数，注入存储库
        public AnnouncementService(IRepository<Announcement> announcementRepository, 
                                   IRepository<Admin> adminRepository, 
                                   IRepository<Venue> venueRepository, 
                                   ApplicationDbContext context)
        {
            _announcementRepository = announcementRepository;
            _adminRepository = adminRepository;
            _venueRepository = venueRepository;
            _context = context; // 注入 ApplicationDbContext
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
        public IEnumerable<PublicNoticeDto> GetPublicNoticeData()
        {
            var notices = _announcementRepository.GetAll()
                .Select(a => new PublicNoticeDto
                {
                    Id = a.AnnouncementId,
                    Title = a.Title,
                    Time = a.PublishedDate,
                    AdminId = a.AdminId
                }).ToList();

            // 如果没有找到任何公告，返回一个空的集合
            if (notices == null || !notices.Any())
            {
                return new List<PublicNoticeDto>();
            }

            return notices;
        }


        public AnnouncementDetailResult GetAllAnnouncementsById(string id)
        {
            // 查找公告信息
            var announcement = _announcementRepository.Find(x => x.AnnouncementId == id).FirstOrDefault();

            if (announcement == null)
            {
                return new AnnouncementDetailResult
                {
                    Status = 0,
                    Info = "未找到该公告",
                    Data = null
                };
            }

            // 查找与公告相关的场地信息
            var venues = _context.VenueAnnouncements
                .Where(va => va.AnnouncementId == id)
                .Select(va => new VenueInfoDto
                {
                    VenueId = va.VenueId,
                    VenueName = _context.Venues.FirstOrDefault(v => v.VenueId == va.VenueId).Name
                })
                .ToList();

            // 查找发布公告的管理员信息
            var admin = _adminRepository.Find(a => a.AdminId == announcement.AdminId).FirstOrDefault();

            // 生成返回的公告详情对象
            var announcementDetails = new AnnouncementDetailsDto
            {
                AnnouncementId = announcement.AnnouncementId,
                Content = announcement.Content,
                AdminName = admin?.RealName,
                Venues = venues
            };

            return new AnnouncementDetailResult
            {
                Status = 1,
                Info = "",
                Data = announcementDetails
            };
        }

    }
}
