using System;

namespace VenueBookingSystem.Models
{
    public class GroupReservationMember
    {
        public required string ReservationId { get; set; } // 预约ID
        public required string GroupId { get; set; } // 团体ID
        public required int NumOfPeople { get; set; } // 预约人数
        public required int ActualNumOfPeople { get; set; } // 实到人数
        public required string Status { get; set; } // 预约状态

        // 导航属性
        public Group? Group { get; set; } // 关联到团体
        public Reservation? Reservation { get; set; } // 关联到预约记录
    }
}