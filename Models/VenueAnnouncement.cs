using Microsoft.EntityFrameworkCore;

namespace VenueBookingSystem.Models
{
    public class VenueAnnouncement
    {
        public string VenueId { get; set; } // 外键关联场地
        public Venue Venue { get; set; }

        public int AnnouncementId { get; set; } // 外键关联公告
        public Announcement Announcement { get; set; }
    }
}
