using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    // 定义 IEventService 接口
    public interface IEventService
    {
        // 创建事件的方法签名
        void CreateEvent(EventDto eventDto);

        // 获取所有事件的方法签名
        IEnumerable<EventDto> GetAllEvents();
    }
}
