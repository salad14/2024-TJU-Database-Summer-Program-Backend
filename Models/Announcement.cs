namespace VenueBookingSystem.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; } // 公告ID
        public required string Title { get; set; }   // 公告标题
        public required string Content { get; set; } // 公告内容
        public DateTime PublishedDate { get; set; } // 发布时间

        public int UserId { get; set; } // 发布公告的用户ID
        public User User { get; set; } // 发布公告的用户

    // 导航属性：场地公告关系
    public ICollection<VenueAnnouncement> VenueAnnouncements { get; set; }
    }
}
