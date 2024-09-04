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
        //管理员添加公告
        [HttpPost("addAnnouncement")]
        public IActionResult AddAnnouncement([FromBody] AddAnnouncementDto announcementDto)
        {
            var result = _announcementService.AddAnnouncement(announcementDto);

            if (result.State == 0)
            {
                return BadRequest(new { Status = 0, Info = result.Info });
            }

            return Ok(new { Status = 1, AnnouncementId = result.AnnouncementId, Info = result.Info });
        }

        // 管理员删除公告
        [HttpPost("deleteAnnouncement")]
        public IActionResult DeleteAnnouncement([FromBody] DeleteAnnouncementRequest request)
        {
            var result = _announcementService.DeleteAnnouncement(request.AnnouncementId);

            if (result.State == 0)
            {
                return BadRequest(new { Status = 0, Info = result.Info });
            }

            return Ok(new { Status = 1, Info = result.Info });
        }



        // 更新公告信息的接口
        [HttpPut("updateAnnouncement")]
        public IActionResult UpdateAnnouncement([FromBody] UpdateAnnouncementDto announcementDto)
        {
            try
            {
                // 调用服务层进行公告的更新
                var result = _announcementService.UpdateAnnouncement(announcementDto);
                
                if (result.State == 1)
                {
                    return Ok(new
                    {
                        Status = 1,
                        Info = result.Info,
                        AnnouncementId = result.AnnouncementId
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Info = result.Info
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Info = $"服务器错误: {ex.Message}"
                });
            }
        }





        // 其他公告相关操作...
    }
}
