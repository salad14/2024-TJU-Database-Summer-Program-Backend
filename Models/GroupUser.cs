namespace VenueBookingSystem.Models
{
    public class GroupUser
    {
        public required string UserId { get; set; }
        public required User User { get; set; }

        public string GroupId { get; set; }
        public required Group Group { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow; // 加入时间

        public string RoleInGroup { get; set; } // 团体中的地位
    }
}
