using System.Collections.Generic;

namespace VenueBookingSystem.Models
{
    public class Venue
    {
        public int VenueId { get; set; } // 场地ID
        public required string Name { get; set; } // 场地名称
        public required string Type { get; set; } // 场地类型
        public int Capacity { get; set; } // 容纳人数

        // 导航属性：场地的预约记录
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
