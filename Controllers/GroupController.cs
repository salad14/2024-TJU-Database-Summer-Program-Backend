using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;
using System.Collections.Generic;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("create")]
        public IActionResult CreateGroup(Group group)
        {
            _groupService.CreateGroup(group);
            return Ok("团体创建成功");
        }

        [HttpGet("{id}")]
        public IActionResult GetGroupById(int id)
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
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            _groupService.AddUserToGroup(groupId, userId);
            return Ok("用户已加入团体");
        }

        [HttpDelete("{groupId}/removeuser/{userId}")]
        public IActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            _groupService.RemoveUserFromGroup(groupId, userId);
            return Ok("用户已从团体中移除");
        }
    }
}
