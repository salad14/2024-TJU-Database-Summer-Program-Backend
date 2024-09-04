namespace VenueBookingSystem.Models
{
    public class Announcement
    {
        public string AnnouncementId { get; set; } // 公告ID
        public required string Title { get; set; }   // 公告标题
        public required string Content { get; set; } // 公告内容
        public DateTime PublishedDate { get; set; } // 发布时间
        public DateTime LastModifiedDate { get; set; } // 最近修改时间

        public required string AdminId { get; set; } // 发布公告的管理员ID
        public Admin Admin { get; set; } // 发布公告的管理员

    // 导航属性：场地公告关系
    public ICollection<VenueAnnouncement> VenueAnnouncements { get; set; }
    }
}
