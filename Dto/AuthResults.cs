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
        public int State { get; set; } // 0为登录失败，1为登录成功
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int IsAdmin { get; set; } // 0为普通用户，1为管理员
        public string Info { get; set; } = string.Empty;
    }
}
