namespace VenueBookingSystem.Models
{
    // 定义 UserDto 类，用于用户注册的数据传输对象
    public class UserDto
    {
        public required string Username { get; set; }  // 用户名
        public required string Password { get; set; }  // 密码
        public required string ContactNumber { get; set; }  // 联系电话
        public bool IsVip { get; set; }  // 是否为VIP用户
    }
}
