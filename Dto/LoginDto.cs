namespace VenueBookingSystem.Models
{
    // 定义 LoginDto 类，用于用户登录的数据传输对象
    public class LoginDto
    {
        public required string Username { get; set; }  // 用户名
        public required string Password { get; set; }  // 密码
    }
}
