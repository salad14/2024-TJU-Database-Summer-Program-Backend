namespace VenueBookingSystem.Models
{
    public class UserDto
    {
        public required string Username { get; set; }  // 用户名
        public required string Password { get; set; }  // 密码
        public required string ContactNumber { get; set; }  // 联系电话
        public required string UserType { get; set; }  // 用户类型
        public bool ReservationPermission { get; set; }  // 预约权限
        public int ViolationCount { get; set; } = 0;  // 违约次数
        public bool IsVip { get; set; }  // VIP状态
        public double DiscountRate { get; set; } = 0.0;  // 折扣力度
         public required string RealName { get; set; }  // 真实姓名
    }
}
