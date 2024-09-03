namespace VenueBookingSystem.Dto
{
    public class LoginDto
    {
        public string? Username { get; set; }  // 用户名（可选）
        public string? UserId { get; set; }  // 用户ID（可选）
        public required string Password { get; set; }  // 密码（必需）
    }
    
}
