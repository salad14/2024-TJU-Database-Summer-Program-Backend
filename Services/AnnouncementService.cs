using System.Collections.Generic;
using System.Linq; // 必需的命名空间，用于 LINQ 操作
using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
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

        public AddAnnouncementResult AddAnnouncement(AddAnnouncementDto announcementDto)
        {
            // 检查管理员是否存在
            var admin = _adminRepository.Find(a => a.AdminId == announcementDto.AdminId).FirstOrDefault();
            if (admin == null)
            {
                return new AddAnnouncementResult
                {
                    State = 0,
                    Info = "管理员ID无效"
                };
            }

            // 生成唯一公告ID
            string announcementId = GenerateUniqueAnnouncementId();

            // 创建并保存公告
            var announcement = new Announcement
            {
                AnnouncementId = announcementId,
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                AdminId = announcementDto.AdminId,
                PublishedDate = DateTime.Now
            };
            _announcementRepository.Add(announcement);

            // 添加场地公告
            if (announcementDto.NoticeVenues != null && announcementDto.NoticeVenues.Any())
            {
                foreach (var venueId in announcementDto.NoticeVenues)
                {
                    // 检查场地是否存在
                    var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
                    if (venue == null)
                    {
                        return new AddAnnouncementResult
                        {
                            State = 0,
                            Info = $"场地ID {venueId} 不存在"
                        };
                    }

                    var venueAnnouncement = new VenueAnnouncement
                    {
                        AnnouncementId = announcementId,
                        VenueId = venueId
                    };
                    _context.VenueAnnouncements.Add(venueAnnouncement);
                }
            }

            // 保存数据库更改
            _context.SaveChanges();

            return new AddAnnouncementResult
            {
                State = 1,
                AnnouncementId = announcementId,
                Info = "公告添加成功"
            };
        }

        public UpdateAnnouncementResult UpdateAnnouncement(UpdateAnnouncementDto announcementDto)
        {
            // 1. 查找公告
            var announcement = _announcementRepository.Find(a => a.AnnouncementId == announcementDto.AnnouncementId).FirstOrDefault();
            if (announcement == null)
            {
                return new UpdateAnnouncementResult
                {
                    State = 0,
                    Info = "公告ID无效,未找到对应公告"
                };
            }

            // 2. 更新公告的基本信息
            announcement.Title = announcementDto.Title;
            announcement.Content = announcementDto.Content;
            announcement.PublishedDate = DateTime.Now;
            _announcementRepository.Update(announcement);

            // 3. 处理场地公告关系
            // 查找现有的场地公告关系
            var currentVenueAnnouncements = _context.VenueAnnouncements
                .Where(va => va.AnnouncementId == announcementDto.AnnouncementId)
                .ToList();

            // 如果前端传递的 noticeVenues 数组为空，删除所有关联
            if (announcementDto.NoticeVenues == null || !announcementDto.NoticeVenues.Any())
            {
                _context.VenueAnnouncements.RemoveRange(currentVenueAnnouncements);
            }
            else
            {
                // 处理要保留的场地公告关系
                var venuesToKeep = announcementDto.NoticeVenues.Intersect(currentVenueAnnouncements.Select(va => va.VenueId)).ToList();

                // 删除不存在于 noticeVenues 中的场地公告
                var venuesToRemove = currentVenueAnnouncements.Where(va => !venuesToKeep.Contains(va.VenueId)).ToList();
                _context.VenueAnnouncements.RemoveRange(venuesToRemove);

                // 添加新的场地公告
                foreach (var venueId in announcementDto.NoticeVenues.Except(venuesToKeep))
                {
                    // 检查场地是否存在
                    var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
                    if (venue == null)
                    {
                        return new UpdateAnnouncementResult
                        {
                            State = 0,
                            Info = $"场地ID {venueId} 不存在"
                        };
                    }

                    var venueAnnouncement = new VenueAnnouncement
                    {
                        AnnouncementId = announcement.AnnouncementId,
                        VenueId = venueId
                    };
                    _context.VenueAnnouncements.Add(venueAnnouncement);
                }
            }

            // 4. 保存所有更改
            _context.SaveChanges();

            return new UpdateAnnouncementResult
            {
                State = 1,
                AnnouncementId = announcement.AnnouncementId,
                Info = "公告更新成功"
            };
        }

        public DeleteAnnouncementResult DeleteAnnouncement(string announcementId)
        {
            var announcement = _announcementRepository.Find(a => a.AnnouncementId == announcementId).FirstOrDefault();
            
            if (announcement == null)
            {
                return new DeleteAnnouncementResult
                {
                    State = 0,
                    Info = "公告未找到"
                };
            }

            _announcementRepository.Delete(announcement);
            
            try
            {
                _context.SaveChanges();
                return new DeleteAnnouncementResult
                {
                    State = 1,
                    Info = "公告删除成功"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAnnouncementResult
                {
                    State = 0,
                    Info = $"删除公告时发生错误：{ex.Message}"
                };
            }
        }




        private string GenerateUniqueAnnouncementId()
        {
            // 查找数据库中最大的公告ID
            var maxId = _announcementRepository.GetAll()
                .OrderByDescending(a => a.AnnouncementId)
                .Select(a => a.AnnouncementId)
                .FirstOrDefault();

            // 如果数据库中没有任何公告，则从 100001 开始
            int newId = 100001;
            if (int.TryParse(maxId, out int currentMaxId))
            {
                newId = currentMaxId + 1;
            }

            return newId.ToString();
        }




    }
}
