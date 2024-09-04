namespace VenueBookingSystem.Models
{
    public class Announcement
    {
<<<<<<< HEAD
=======
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        public string AnnouncementId { get; set; } // 公告ID
        public required string Title { get; set; }   // 公告标题
        public required string Content { get; set; } // 公告内容
        public DateTime PublishedDate { get; set; } // 发布时间
        public DateTime LastModifiedDate { get; set; } // 最近修改时间

        public required string AdminId { get; set; } // 发布公告的管理员ID
        public Admin Admin { get; set; } // 发布公告的管理员
<<<<<<< HEAD
=======
=======
=======
>>>>>>> 6124219dafaa70ec3e921df99c0e4f6746323204
        public int AnnouncementId { get; set; } // 公告ID
        public required string Title { get; set; }   // 公告标题
        public required string Content { get; set; } // 公告内容
        public DateTime PublishedDate { get; set; } // 发布时间

        public int UserId { get; set; } // 发布公告的用户ID
        public User User { get; set; } // 发布公告的用户
<<<<<<< HEAD
>>>>>>> 6124219dafaa70ec3e921df99c0e4f6746323204
=======
>>>>>>> 6124219dafaa70ec3e921df99c0e4f6746323204
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99

    // 导航属性：场地公告关系
    public ICollection<VenueAnnouncement> VenueAnnouncements { get; set; }
    }
}
