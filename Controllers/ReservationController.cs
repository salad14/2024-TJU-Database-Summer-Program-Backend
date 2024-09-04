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

    }
}
