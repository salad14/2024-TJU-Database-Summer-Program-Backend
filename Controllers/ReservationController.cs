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
                return NotFound("δ�ҵ���Ӧ��ԤԼ��¼");
            }

            return Ok("ԤԼ״̬���³ɹ�");
        }

        [HttpPost("GetReservationsByVenueIds")]
        public IActionResult GetReservationsByVenueIds([FromBody] VenueIdsDto venueIdsDto)
        {
            if (venueIdsDto == null || !venueIdsDto.VenueIds.Any())
            {
                return BadRequest("The venueIds field is required.");
            }

            var result = _reservationService.GetReservationsByVenueIds(venueIdsDto.VenueIds);

            // ������Ϊ�գ����ؿ�����
            if (result == null || !result.Any())
            {
                return Ok(new List<ReservationDetailDto>());  // ����һ�������� []
            }

            return Ok(result);  // �������ؽ��
        }


        [HttpGet("GetReservationsByUserId")]
        public IActionResult GetReservationsByUserId(string userId)
        {
            var result = _reservationService.GetReservationsByUserId(userId);
            if (result == null || !result.Any())
            {
                return NotFound("δ�ҵ����û���ԤԼ��¼��");
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
                return NotFound("δ�ҵ���ԤԼ��¼�ĳ�Ա��Ϣ��");
            }

            return Ok(result);
        }



    }
}
