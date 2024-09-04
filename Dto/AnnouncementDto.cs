namespace VenueBookingSystem.Models
{
    public class AnnouncementDto
    {
        public required string Title { get; set; }  // 公告标题
        public required string Content { get; set; }  // 公告内容
        public DateTime PublishDate { get; set; }  // 发布日期

        public required string AdminId { get; set; }  // 发布公告的管理员ID
    }

    public class AnnouncementVenueDto
    {
        public required string Title { get; set; }  // 公告标题
        public required string Content { get; set; }  // 公告内容
        public string PublishDate { get; set; }  // 发布日期

        public required string AdminId { get; set; }  // 发布公告的管理员ID

        public List<Venue> Venues { get; set; }
    }
}
