using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        // 构造函数，注入事件服务
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // 创建新事件
        [HttpPost]
        public IActionResult CreateEvent(EventDto eventDto)
        {
            _eventService.CreateEvent(eventDto);
            return Ok("事件创建成功");
        }

        // 获取所有事件
        [HttpGet]
        public IActionResult GetAllEvents()
        {
            var events = _eventService.GetAllEvents();
            return Ok(events);
        }

        // 其他事件相关操作...
    }
}
