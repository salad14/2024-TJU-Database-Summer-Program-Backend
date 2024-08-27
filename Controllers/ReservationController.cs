using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [EnableCors("_allowSpecificOrigins")]
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

        // 其他预约相关操作...
    }
}
