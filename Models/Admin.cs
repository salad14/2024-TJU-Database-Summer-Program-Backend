namespace VenueBookingSystem.Models
{
    public class Admin
    {
        public required string AdminId { get; set; }
        public string RealName { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }

        public required string AdminType { get; set; }

        // 导航属性：管理员的通知记录
        public ICollection<AdminNotification> AdminNotifications { get; set; } = new List<AdminNotification>();
    }
}