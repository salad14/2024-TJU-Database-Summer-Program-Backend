using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
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
        [HttpGet("publicNoticeData")]
        public IActionResult GetPublicNoticeData()
        {
            try
            {
                var result = _announcementService.GetPublicNoticeData();

                if (result == null || !result.Any())
                {
                    return Ok(new 
                    {
                        Status = 1,  // 状态为1表示成功
                        Info = "没有公告数据",
                        Data = new List<PublicNoticeDto>() // 返回一个空的结果集
                    });
                }

                return Ok(new 
                {
                    Status = 1,  // 状态为1表示成功
                    Info = "",   // Info为空字符串表示成功
                    Data = result // 返回公告数据
                });
            }
            catch (Exception ex)
            {
                // 处理获取数据时的任何异常，返回错误信息
                return BadRequest(new 
                { 
                    Status = 0,  // 状态为0表示失败
                    Info = $"获取公告数据失败: {ex.Message}",
                    Data = new List<PublicNoticeDto>() // 返回一个空的结果集
                });
            }
        }


        // 获取某个公告下的详细信息
        [HttpGet("getAnnouncementDetails/{id}")]
        public IActionResult GetAnnouncementDetails(string id)
        {
            var result = _announcementService.GetAllAnnouncementsById(id);

            if (result == null || result.Data == null)
            {
                return NotFound(new 
                {
                    Status = 0, 
                    Info = "未找到相关公告详情"
                });
            }

            return Ok(result);
        }




        // 其他公告相关操作...
    }
}
