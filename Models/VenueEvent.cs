namespace VenueBookingSystem.Models
{
    public class VenueEvent
    {
        public int VenueId { get; set; } // 外键关联场地
        public Venue Venue { get; set; }

        public int EventId { get; set; } // 外键关联事件
        public Event Event { get; set; }
    }
}
