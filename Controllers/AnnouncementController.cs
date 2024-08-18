using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        // 构造函数，注入公告服务
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // 发布新公告
        [HttpPost]
        public IActionResult PublishAnnouncement(AnnouncementDto announcementDto)
        {
            _announcementService.PublishAnnouncement(announcementDto);
            return Ok("公告发布成功");
        }

        // 获取所有公告
        [HttpGet]
        public IActionResult GetAllAnnouncements()
        {
            var announcements = _announcementService.GetAllAnnouncements();
            return Ok(announcements);
        }

        // 其他公告相关操作...
    }
}
