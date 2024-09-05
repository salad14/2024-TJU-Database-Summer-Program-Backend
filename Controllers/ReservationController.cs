using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("createUserReservation")]
        public IActionResult CreateReservation([FromBody] ReservationDto reservationDto,[FromQuery]string UserId)
        {
            var result = _reservationService.CreateReservation(reservationDto,UserId);
            if (result.State == 1)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("createGroupReservation")]
        public IActionResult CreateGroupReservation([FromBody] GroupReservationDto groupReservationDto)
        {
            var result = _reservationService.CreateGroupReservation(groupReservationDto);
            if (result.State == 1)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // 根据预约记录ID，查找团体预约每个成员的预约信息
        [HttpGet("getReservationUser")]
        public IActionResult GetReservationUser(string reservationId)
        {
          var userList =_reservationService.GetReservationUser(reservationId);
            return Ok(userList);
        }

        // 预约记录修改
        [HttpPost("updateReservationUser")]
        public IActionResult UpdateReservationUser(UpdateReservationUserDto req)
        {
            try
            {
                _reservationService.UpdateReservationUser(req);
                return Ok("更新成功");

            }
            catch
            {
                return Ok("更新失败");
            }
        }
        
        // 获取所有预约记录
        [HttpGet("getReservationList")]
        public IActionResult GetReservationList()
        {
            var reservationList = _reservationService.GetReservationList();
            return Ok(reservationList);
        }

        // 根据场地ID获取预约记录
        [HttpGet("getReservationVenueList")]
        public IActionResult GetReservationVenueList(string venueId)
        {
            var reservationList = _reservationService.GetReservationVenueList(venueId);
            return Ok(reservationList);
        }



    }
}
