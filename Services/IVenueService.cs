using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    // 定义 IVenueService 接口
    public interface IVenueService
    {
        // 获取所有场地的方法签名
        IEnumerable<Venue> GetAllVenues();

        // 添加新场地的方法签名
        void AddVenue(VenueDto venueDto);
<<<<<<< HEAD

        // 获取所有不同ID的场地
        IEnumerable<VenueDto> GetAllVenueDetails();

        // 获取指定场地的详细信息
        VenueDetailsDto GetVenueDetails(string venueId);

        // 获取所有维修记录以及相关设备和场地信息
        IEnumerable<RepairDataDto> GetAllRepairRecords();
        // 获取指定设备的详细信息
        DeviceDetailsDto GetDeviceDetails(string equipmentId);
        // 获取所有场地信息的方法签名
        IEnumerable<VenueDto> GetAllVenueInfos();

        // 获取所有场地信息详情公告
        VenueAnnouncementDto GetVenueDetailsAnnouncement(string venueId);
=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
    }
}
