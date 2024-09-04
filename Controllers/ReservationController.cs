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


        [HttpPost("UpdateReservationStatus")]
        public IActionResult UpdateReservationStatus([FromBody] ReservationStatusUpdateDto updateRequest)
        {
            var result = _reservationService.UpdateReservationStatus(
                updateRequest.ReservationId,
                updateRequest.UserId,
                updateRequest.CheckInTime,
                updateRequest.Status
            );

            if (!result)
            {
                return NotFound("未找到对应的预约记录");
            }

            return Ok("预约状态更新成功");
        }

        [HttpPost("GetReservationsByVenueIds")]
        public IActionResult GetReservationsByVenueIds([FromBody] VenueIdsDto venueIdsDto)
        {
            if (venueIdsDto == null || !venueIdsDto.VenueIds.Any())
            {
                return BadRequest("The venueIds field is required.");
            }

            var result = _reservationService.GetReservationsByVenueIds(venueIdsDto.VenueIds);

            // 如果结果为空，返回空数组
            if (result == null || !result.Any())
            {
                return Ok(new List<ReservationDetailDto>());  // 返回一个空数组 []
            }

            return Ok(result);  // 正常返回结果
        }


        [HttpGet("GetReservationsByUserId")]
        public IActionResult GetReservationsByUserId(string userId)
        {
            var result = _reservationService.GetReservationsByUserId(userId);
            if (result == null || !result.Any())
            {
                return NotFound("未找到该用户的预约记录。");
            }

            return Ok(result);
        }



        [HttpGet("GetAllReservations")]
        public IActionResult GetAllReservations()
        {
            var result = _reservationService.GetAllReservations();
            return Ok(result);
        }


        [HttpGet("GetGroupReservationMembers")]
        public IActionResult GetGroupReservationMembers(string reservationId)
        {
            var result = _reservationService.GetGroupReservationMembers(reservationId);
            if (result == null || result.Count == 0)
            {
                return NotFound("未找到该预约记录的成员信息。");
            }

            return Ok(result);
        }



    }
}
