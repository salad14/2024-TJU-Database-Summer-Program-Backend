using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sports_management.Dto;
using sports_management.Services;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace sports_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class UserReservationController : ControllerBase
    {
        private readonly IUserReservationService _userReservationService;
        public UserReservationController(IUserReservationService userReservationService)
        {
            _userReservationService= userReservationService;
        }

        //更新用户违约状态
        [HttpGet("UpdateUserViolation")]
        public IActionResult UpdateUserViolation([FromQuery] string userId)
        {
            try
            {
                _userReservationService.UpdateUserViolation(userId);
                return Ok("操作成功");
            }
            catch (Exception ex)
            {
                return Ok("操作失败");
            }

        }



    }
}
