using System;

namespace VenueBookingSystem.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; } // 预约ID
        public DateTime StartTime { get; set; } // 预约开始时间
        public DateTime EndTime { get; set; } // 预约结束时间
        public decimal AmountPaid { get; set; } // 支付金额

        // 导航属性
        public int UserId { get; set; } // 关联的用户ID
        public required User User { get; set; } // 关联的用户
        public int VenueId { get; set; } // 关联的场地ID
        public required Venue Venue { get; set; } // 关联的场地
        
    }
}
