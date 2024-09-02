using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    // 定义 IAnnouncementService 接口
    public interface IAnnouncementService
    {
        // 发布公告的方法签名
        void PublishAnnouncement(AnnouncementDto announcementDto);

        // 获取所有公告的方法签名
        IEnumerable<AnnouncementDto> GetAllAnnouncements();

        AnnouncementVenueDto GetAllAnnouncementsById(string Id);
    }
}
