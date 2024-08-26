namespace VenueBookingSystem.Models
{
    public class AnnouncementDto
    {
        public required string Title { get; set; }  // 公告标题
        public required string Content { get; set; }  // 公告内容
        public DateTime PublishDate { get; set; }  // 发布日期

        public string UserId { get; set; }  
    }
}
