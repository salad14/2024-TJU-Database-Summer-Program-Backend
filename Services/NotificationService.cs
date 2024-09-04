using System;

namespace VenueBookingSystem.Services
{
    // 定义INotificationService接口
    public interface INotificationService
    {
        // 发送通知的方法签名
        void SendNotification(string message, int userId);
    }

    // 实现INotificationService接口的NotificationService类
    public class NotificationService : INotificationService
    {
        // 实现接口中的SendNotification方法
        public void SendNotification(string message, int userId)
        {
            // 通知逻辑，例如发送邮件或短信
            Console.WriteLine($"向用户 {userId} 发送通知: {message}");
        }

        // 其他通知服务逻辑...
    }
}
