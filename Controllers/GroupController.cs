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
    [EnableCors("AllowSpecificOrigins")]
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


        [HttpPost("{groupId}/adduser")]
        public IActionResult AddUserToGroup(string groupId, [FromBody] UserJoinGroupDto userJoinGroupDto)
        {
            var result = _groupService.AddUserToGroup(
                groupId,
                userJoinGroupDto.UserId,
                userJoinGroupDto.JoinDate,
                userJoinGroupDto.RoleInGroup
            );
            return Ok(result);
        }


        [HttpDelete("{groupId}/removeuser")]
        public IActionResult RemoveUserFromGroup(string groupId, [FromBody] RemoveUserDto removeUserDto)
        {
            var result = _groupService.RemoveUserFromGroup(groupId, removeUserDto.UserId, removeUserDto.AdminId);
            return Ok(result);
        }


        [HttpGet("userallGroup/{userId}")]
        public IActionResult UserAllGroups(string userId)
        {
            var userGroups = _groupService.UserAllGroups(userId);

            // 检查是否没有找到任何团体
            if (!userGroups.Any())
            {
                // 如果没有找到，返回一个包含空字段的 UserGroupDto 数组
                return Ok(new
                {
                    status = 1,
                    message = "获取成功",
                    userGroups = new List<UserGroupDto>(),
                });
            }
            return Ok(new
            {
                status = 1,
                message = "获取成功",
                userGroups = userGroups

            });
        }



        [HttpGet("selectGroups")]
        public IActionResult SelectGroups()
        {
            var groups = _groupService.SelectGroups();
            return Ok(groups);
        }

        [HttpPost("updateUserRole")]
        public IActionResult UpdateUserRoleInGroup([FromBody] UpdateUserRoleDto updateUserRoleDto)
        {
            var result = _groupService.UpdateUserRoleInGroup(
                updateUserRoleDto.GroupId,
                updateUserRoleDto.UserId,
                updateUserRoleDto.UserRole,
                updateUserRoleDto.AdminId,
                updateUserRoleDto.NotificationType
            );

            return Ok(result);
        }

        [HttpGet("details/{groupId}")]
        public IActionResult GetGroupDetails(string groupId)
        {
            var groupDetail = _groupService.GetGroupDetailById(groupId);
            if (groupDetail == null)
            {
                return NotFound(new
                {
                    status = 0,
                    message = "未找到该团体"
                });
            }
            return Ok(new
            {
                status = 1,
                message = "获取成功",
                data = groupDetail
            });
        }

        [HttpPost("updateGroupInfo")]
        public IActionResult UpdateGroupInfo([FromBody] GroupUpdateDto groupUpdateDto)
        {
            var result = _groupService.UpdateGroupInfo(
                groupUpdateDto.GroupId,
                groupUpdateDto.GroupName,
                groupUpdateDto.Description
            );

            if (result)
            {
                return Ok(new { status = 1, message = "团体信息更新成功" });
            }
            else
            {
                return NotFound(new { status = 0, message = "未找到指定的团体" });
            }
        }

    }
}
