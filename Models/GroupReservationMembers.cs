using System;

namespace VenueBookingSystem.Models
{
    public class GroupReservationMember
    {
        public required string ReservationId { get; set; } // 预约ID
        public required string GroupId { get; set; } // 团体ID

        // 导航属性
        public Group? Group { get; set; } // 关联到团体
        public Reservation? Reservation { get; set; } // 关联到预约记录
    }
}