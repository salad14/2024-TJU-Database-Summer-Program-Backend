namespace VenueBookingSystem.Dto
{
    public class UserPersonalInfoDto
    {
        public string UserId { get; set; } // 用户ID
        public string Username { get; set; } // 用户名
        public string ContactNumber { get; set; } // 联系电话
        public int ViolationCount { get; set; } // 违约次数
        public string ReservationPermission { get; set; } // 预约权限
    }
}