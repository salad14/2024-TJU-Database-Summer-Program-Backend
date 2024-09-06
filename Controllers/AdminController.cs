using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Data;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ApplicationDbContext _context;

        

        public AdminController(IAdminService adminService,ApplicationDbContext context)
        {
            _adminService = adminService;
            _context = context;
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

        [HttpGet("{adminId}/managedItems")]
        public IActionResult GetAdminManagedVenuesAndEquipment(string adminId)
        {
            try
            {
                var result = _adminService.GetAdminManagedVenuesAndEquipment(adminId);
                return Ok(new
                {
                    Status = 1,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Info = $"获取管理员管理的场地和设备失败: {ex.Message}"
                });
            }
        }

        [HttpGet("{adminId}/info")]
        public IActionResult GetAdminInfo(string adminId)
        {
            try
            {
                var result = _adminService.GetAdminInfo(adminId);
                return Ok(new
                {
                    Status = 1,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Info = $"获取管理员信息失败: {ex.Message}"
                });
            }
        }


        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] AdminRegistrationDto registrationDto)
        {
            try
            {
                // 传入 systemAdminId
                var result = _adminService.RegisterAdmin(
                    registrationDto.AdminDto, 
                    registrationDto.ManageVenues, 
                    registrationDto.SystemAdminId
                );
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

        // 获取所有管理员信息
        [HttpGet("allAdmins")]
        public IActionResult GetAllAdmins()
        {
            try
            {
                // 查询所有管理员信息
                var admins = _context.Admins.Select(admin => new AdminResponseDto
                {
                    RealName = admin.RealName,
                    ContactNumber = admin.ContactNumber,
                    AdminType = admin.AdminType
                }).ToList();

                return Ok(new
                {
                    State = 1,
                    Info = "成功获取管理员信息",
                    Data = admins
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    State = 0,
                    Info = $"获取管理员信息失败: {ex.Message}"
                });
            }
        }

        // 其他的 Admin 相关方法可以类似于此处添加
    }
}
