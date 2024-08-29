using System;

namespace VenueBookingSystem.Models
{
    public class UserNotification
    {
        public string UserId { get; set; }  // 用户ID
        public required string NotificationId { get; set; }  // 通知ID
        public string NotificationType { get; set; }  // 通知类型
        public string Title { get; set; }  // 通知标题
        public string Content { get; set; }  // 通知内容
        public DateTime NotificationTime { get; set; }  // 通知时间

        // 导航属性：关联到用户
        public User? User { get; set; }
    }
}
