using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;
using VenueBookingSystem.Dto;

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

        [HttpPost("AdminNoticeData")]
        public IActionResult GetAdminNoticeData([FromBody] AdminRequestDto request)
        {
            try
            {
                var result = _adminService.GetAdminNoticeData(request.AdminId);

                if (result == null || !result.Any())
                {
                    return Ok(new 
                    {
                        Status = 1,  // 状态为1表示成功
                        Info = "没有通知数据",
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
                    Info = $"获取通知数据失败: {ex.Message}" 
                });
            }
        }

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

        [HttpPut("updateAdminInfo/{adminId}")]
        public IActionResult UpdateAdminInfo(string adminId, [FromBody] AdminUpdateDto adminUpdateDto, [FromQuery] List<string> manageVenues)
        {
            var result = _adminService.UpdateAdminInfo(adminId, adminUpdateDto, manageVenues);
            return Ok(result);
        }


        [HttpPut("updatePassword")]
        public IActionResult UpdateAdminPassword([FromQuery] string adminId, [FromBody] UpdateAdminPasswordDto updateDto)
        {
            var result = _adminService.UpdateAdminPassword(adminId, updateDto.NewPassword);
            return Ok(result);
        }





        // 其他的 Admin 相关方法可以类似于此处添加
    }
}
