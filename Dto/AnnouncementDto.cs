namespace VenueBookingSystem.Models
{
    public class AnnouncementDto
    {
        public required string Title { get; set; }  // 公告标题
        public required string Content { get; set; }  // 公告内容
        public DateTime PublishDate { get; set; }  // 发布日期

        public string? AdminId { get; set; }  // 发布公告的管理员ID
    }

    public class AnnouncementDetailsDto
    {
        public string AnnouncementId { get; set; }
        public string Content { get; set; }
        public List<VenueInfoDto> Venues { get; set; }
        public string AdminName { get; set; }
    }

    public class AnnouncementVenueDto
    {
        public required string Title { get; set; }  // 公告标题
        public required string Content { get; set; }  // 公告内容
        public string PublishDate { get; set; }  // 发布日期

        public required string AdminId { get; set; }  // 发布公告的管理员ID

        public List<Venue> Venues { get; set; }
    }

    public class PublicNoticeDto
    {
        public string Id { get; set; }         // 公告ID
        public string Title { get; set; }      // 公告标题
        public DateTime Time { get; set; }     // 发布时间
        public string AdminId { get; set; }    // 管理员ID
    }

    public class AnnouncementDetailResult
    {
        public int Status { get; set; }
        public string Info { get; set; }
        public AnnouncementDetailsDto Data { get; set; }
    }

    public class UpdateAnnouncementDto
    {
        public string AnnouncementId { get; set; }  // 公告ID
        public string Title { get; set; }  // 公告标题
        public string Content { get; set; }  // 公告内容
        public List<string> NoticeVenues { get; set; }  // 场地ID列表
    }

    public class DeleteAnnouncementRequest
    {
        public string AnnouncementId { get; set; }
    }



}
