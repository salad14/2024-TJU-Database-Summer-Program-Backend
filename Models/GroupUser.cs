namespace VenueBookingSystem.Models
{
    public class GroupUser
    {
        public required string UserId { get; set; }
        public required User User { get; set; }

        public int GroupId { get; set; }
        public required Group Group { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow; // 加入时间
    }
}
