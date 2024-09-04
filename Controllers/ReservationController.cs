using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
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

        // 根据预约记录ID，查找团体预约每个成员的预约信息
        [HttpGet]
        public IActionResult GetReservationUser(string reservationId)
        {
          var userList =_reservationService.GetReservationUser(reservationId);
            return Ok(userList);
        }
        /// <summary>
        ///  预约记录修改 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
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
        
        //获取所有预约记录
        [HttpGet]
        public IActionResult GetReservationList()
        {
            var reservationList = _reservationService.GetReservationList();
            return Ok(reservationList);
        }

        //根据场地ID获取预约记录
        [HttpGet]
        public IActionResult GetReservationVenueList(string venueId)
        {
            var reservationList = _reservationService.GetReservationVenueList(venueId);
            return Ok(reservationList);
        }

        // 其他预约相关操作...
    }
}
