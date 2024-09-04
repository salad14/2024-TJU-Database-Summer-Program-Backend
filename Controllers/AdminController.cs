using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;
<<<<<<< HEAD
using VenueBookingSystem.Dto;
=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("publicNoticeData")]
        public IActionResult GetPublicNoticeData([FromBody] AdminRequestDto request)
        {
            try
            {
                // 调用服务层获取公告及相关场地信息
                var result = _adminService.GetPublicNoticeData(request.AdminId);

                if (result == null || !result.Any())
                {
                    return Ok(new 
                    {
                        Status = 1,  // 状态为1表示成功
                        Info = "没有公告数据",
                        Data = result  // 返回空的结果集
                    });
                }
                return Ok(result); // 直接返回公告及场地信息
            }
            catch (Exception ex)
            {
                // 处理获取数据时的任何异常，返回错误信息
                return BadRequest(new 
                { 
                    Status = 0, 
                    Info = $"获取公告数据失败: {ex.Message}" 
                });
            }
        }

<<<<<<< HEAD
        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] AdminDto adminDto, [FromQuery] List<string> manageVenues)
        {
            try
            {
                var result = _adminService.RegisterAdmin(adminDto, manageVenues);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new RegisterResult
                {
                    State = 0,
                    AdminId = string.Empty,
                    Info = $"注册失败: {ex.Message}"
                });
            }
        }


=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99

        // 其他的 Admin 相关方法可以类似于此处添加
    }
}
