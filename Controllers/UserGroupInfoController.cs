using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Services;
using VenueBookingSystem.Dto;
using System.Collections.Generic;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGroupInfoController : ControllerBase
    {
        private readonly IUserGroupInfoService _userGroupInfoService;

        public UserGroupInfoController(IUserGroupInfoService userGroupInfoService)
        {
            _userGroupInfoService = userGroupInfoService;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<UserPersonalGroupInfoDto>> GetUserGroups(string userId)
        {
            try
            {
                var userGroups = _userGroupInfoService.GetUserGroups(userId);
                if (userGroups == null || userGroups.Count == 0)
                {
                    return NotFound(new { Message = "用户没有关联的团体信息" });
                }
                return Ok(userGroups);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}