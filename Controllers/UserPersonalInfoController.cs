﻿using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Services;
using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPersonalInfoController : ControllerBase
    {
        private readonly IUserPersonalInfoService _userPersonalInfoService;

        public UserPersonalInfoController(IUserPersonalInfoService userPersonalInfoService)
        {
            _userPersonalInfoService = userPersonalInfoService;
        }

        [HttpGet("{userId}")]
        public ActionResult<UserPersonalInfoDto> GetUserPersonalInfo(string userId)
        {
            try
            {
                var user = _userPersonalInfoService.GetUserPersonalInfo(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("all-users")]
        public ActionResult<List<UserIdNameDto>> GetAllUserIdsAndNames()
        {
            try
            {
                var users = _userPersonalInfoService.GetAllUserIdsAndNames();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}