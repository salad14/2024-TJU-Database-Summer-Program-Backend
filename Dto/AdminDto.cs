namespace VenueBookingSystem.Models
{
<<<<<<< HEAD

    public class AdminDto
    {
        public string RealName { get; set; }  // 管理员真实姓名
        public string Password { get; set; }  // 管理员密码（已加密）
        public string ContactNumber { get; set; }  // 管理员联系电话
        public string AdminType { get; set; }  // 管理员类型，如 admin-validate/system
    }

    public class AdminRequestDto
    {
        public string AdminId { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
        public string? AdminType { get; set; }
=======
    public class AdminRequestDto
    {
        public string AdminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
    }

    public class AdminNotificationDto
{
    public required string NotificationId { get; set; } // 通知ID
    public string NotificationType { get; set; } // 通知类型
    public string Title { get; set; } // 通知标题
    public string Content { get; set; } // 通知内容
    public DateTime NotificationTime { get; set; } // 通知时间
}

}
