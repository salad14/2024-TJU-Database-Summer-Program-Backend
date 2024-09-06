using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    // 定义 IAnnouncementService 接口
    public interface IAnnouncementService
    {
        IEnumerable<PublicNoticeDto> GetPublicNoticeData();
        AnnouncementDetailResult GetAllAnnouncementsById(string id);
        //管理员添加公告
        AddAnnouncementResult AddAnnouncement(AddAnnouncementDto announcementDto);

        //管理员删除公告
        DeleteAnnouncementResult DeleteAnnouncement(string announcementId);


        UpdateAnnouncementResult UpdateAnnouncement(UpdateAnnouncementDto announcementDto);
    }
}
