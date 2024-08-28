using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;
using System;
using VenueBookingSystem.Models;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using VenueBookingSystem.Data;


namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("_allowSpecificOrigins")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("create")]
        public IActionResult CreateGroup([FromBody] GroupDto groupDto)
        {
            try
            {
                // 调用服务层进行团体创建
                var result = _groupService.CreateGroup(groupDto);

                return Ok(result); // 直接返回 GroupCreateResult 对象
            }
            catch (Exception ex)
            {
                // 处理团体创建时的任何异常，返回失败的 GroupCreateResult
                return BadRequest(new GroupCreateResult 
                { 
                    State = 0, 
                    GroupId = null, 
                    Info = $"团体创建失败: {ex.Message}" 
                });
            }
        }



        [HttpGet("{id}")]
        public IActionResult GetGroupById(string id)
        {
            var group = _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound("未找到该团体");
            }
            return Ok(group);
        }

        [HttpGet]
        public IActionResult GetAllGroups()
        {
            var groups = _groupService.GetAllGroups();
            return Ok(groups);
        }

        [HttpPost("{groupId}/adduser/{userId}")]
        public IActionResult AddUserToGroup(string groupId, string userId)
        {
            _groupService.AddUserToGroup(groupId, userId);
            return Ok("用户已加入团体");
        }

        [HttpDelete("{groupId}/removeuser/{userId}")]
        public IActionResult RemoveUserFromGroup(string groupId, string userId)
        {
            _groupService.RemoveUserFromGroup(groupId, userId);
            return Ok("用户已从团体中移除");
        }

        [HttpGet("userallGroup/{userId}")]
        public IActionResult UserAllGroups(string userId)
        {
            var groups = _groupService.UserAllGroups(userId);
            if (!groups.Any())
            {
                return NotFound("该用户未加入任何团体");
            }
            return Ok(groups);
        }

        [HttpGet("selectGroups")]
        public IActionResult SelectGroups()
        {
            var groups = _groupService.SelectGroups();
            return Ok(groups);
        }
    }
}
