namespace VenueBookingSystem.Models
{
    public class AdminRequestDto
    {
        public string AdminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
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
