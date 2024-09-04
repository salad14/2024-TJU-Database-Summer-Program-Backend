using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        // 构造函数，注入预约服务
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // 创建新预约
        [HttpPost]
        public IActionResult CreateReservation(ReservationDto reservationDto)
        {
            _reservationService.CreateReservation(reservationDto);
            return Ok("预约创建成功");
        }

        // 取消预约
        [HttpPost]
        public IActionResult CancelReservation(int reservationId)
        {
            _reservationService.CancelReservation(reservationId);
            return Ok("预约取消成功");
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

        // 其他预约相关操作...
    }

}
