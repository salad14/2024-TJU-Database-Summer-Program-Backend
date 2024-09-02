using System;
using System.Collections.Generic;
//场地开放时间表
namespace VenueBookingSystem.Models
{
    public class VenueAvailability
    {
        public string AvailabilityId { get; set; }  // 主键
        public required string VenueId { get; set; } // 场地ID，外键
        public required DateTime StartTime { get; set; } // 开始时间
        public required DateTime EndTime { get; set; } // 结束时间
        public required decimal Price { get; set; } // 该时间段的费用
        public required int RemainingCapacity { get; set; } // 剩余容量

        // 导航属性：场地
        public Venue? Venue { get; set; } // 关联到场地

        // 导航属性：与此时间段关联的预约记录
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
