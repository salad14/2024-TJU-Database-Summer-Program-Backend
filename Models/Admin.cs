namespace VenueBookingSystem.Models
{
    public class Admin
    {
        public required string AdminId { get; set; }
<<<<<<< HEAD
        public string RealName { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }

        public required string AdminType { get; set; }

=======
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }

>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        // 导航属性：管理员的通知记录
        public ICollection<AdminNotification> AdminNotifications { get; set; } = new List<AdminNotification>();
    }
}