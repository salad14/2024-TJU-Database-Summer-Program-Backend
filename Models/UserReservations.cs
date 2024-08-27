using System;

namespace VenueBookingSystem.Models
{
    public class UserReservation
    {
        public required string ReservationId { get; set; } // 预约信息ID
        public required string UserId { get; set; } // 用户ID
        public DateTime? CheckInTime { get; set; } // 用户签到时间
        public required string Status { get; set; } // 预约状态

        // 导航属性
        public User User { get; set; } // 关联到用户
        public Reservation Reservation { get; set; } // 关联到预约记录
    }
}
