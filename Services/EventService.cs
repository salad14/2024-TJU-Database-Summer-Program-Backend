using System.Collections.Generic;
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;

        // 构造函数，注入存储库
        public EventService(IRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // 创建事件
        public void CreateEvent(EventDto eventDto)
        {
            var newEvent = new Event
            {
                Name = eventDto.Name,
                Description = eventDto.Description,
                EventDate = eventDto.Date
            };
            _eventRepository.Add(newEvent);
        }

        // 获取所有事件
        public IEnumerable<EventDto> GetAllEvents()
        {
            var events = _eventRepository.GetAll();
            return events.Select(e => new EventDto
            {
                Name = e.Name,
                Description = e.Description,
                Date = e.EventDate
            });
        }
    }
}
