using System;

namespace VenueBookingSystem.Models
{
    public class AdminNotification
    {
        public required string NotificationId { get; set; } // 通知ID
        public string AdminId { get; set; } // 管理员ID
        public string? NewAdminId { get; set; } // 新注册的管理员ID
        public string NotificationType { get; set; } = string.Empty; // 通知类型
        public string Title { get; set; } = string.Empty; // 通知标题
        public string Content { get; set; } = string.Empty; // 通知内容
        public DateTime NotificationTime { get; set; } = DateTime.Now; // 通知时间

        // 导航属性：关联到管理员
        public Admin? Admin { get; set; }
    }
}
