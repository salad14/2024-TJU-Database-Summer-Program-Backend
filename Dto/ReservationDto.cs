namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public decimal PaymentAmount { get; set; }  
        public string UserId { get; set; }  
        public string VenueId { get; set; }  
        public string AvailabilityId { get; set; }  
        public string ReservationItem { get; set; }  
        
        public required string ReservationType { get; set; }  
    }



    public class UserReservationInfoDto
    {
        public string UserId { get; set; } // 用户ID
        public string Username { get; set; } // 用户名
        public string RealName { get; set; } // 真实姓名
        public DateTime? CheckInTime { get; set; } // 签到时间
        public string Status { get; set; } // 预约状态
    }

}
