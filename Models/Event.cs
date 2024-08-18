namespace VenueBookingSystem.Models
{
    public class Event
    {
        public int EventId { get; set; } // 事件ID
        public required string Name { get; set; } // 事件名称
        public required string Description { get; set; } // 事件描述
        public DateTime EventDate { get; set; } // 事件日期

        public int VenueId { get; set; } // 关联的场地ID
         public Venue? Venue { get; set; }  // 可为空的场地属性
    }
}
