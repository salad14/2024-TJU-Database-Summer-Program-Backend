namespace VenueBookingSystem.Dto
{
    // 注册结果类
    public class RegisterResult
    {
        public int State { get; set; } // 0为注册失败，1为注册成功
        public string UserId { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }

    // 登录结果类
    public class LoginResult
    {
    public int State { get; set; }  // 登录状态，1 表示成功，0 表示失败
    public required string UserId { get; set; }  // 用户ID
    public required string UserName { get; set; }  // 用户名
    public required string UserType { get; set; }  // 用户类型
    public required string Info { get; set; }  // 额外的信息，通常用于存储错误消息
    }

}
