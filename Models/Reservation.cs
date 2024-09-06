using System;
// 预约记录表建模
namespace VenueBookingSystem.Models
{
    public class Reservation
    {
        public required string ReservationId { get; set; } // 预约ID
        public required string VenueId { get; set; } // 场地ID
        public required string AvailabilityId { get; set; } // 开放时间段ID
        public required string ReservationItem { get; set; } // 预约项目的描述
        public required DateTime ReservationTime { get; set; } = DateTime.Now; // 预约操作时间
        public required decimal PaymentAmount { get; set; } // 支付金额
        public required string ReservationType { get; set; }  //预约类型（用户，团体）

        // 导航属性：场地 (一对多)
        public Venue? Venue { get; set; } // 可空，ORM 将填充

        // 导航属性：开放时间段 (一对多)
        public VenueAvailability? VenueAvailability { get; set; } 

        // 导航属性：用户的预约记录 (多对多)
        public ICollection<UserReservation>? UserReservations { get; set; } // 可空，ORM 将填充

         // 导航属性：团体预约成员记录 (多对多)
        public ICollection<GroupReservationMember>? GroupReservationMembers { get; set; } 
    }
}