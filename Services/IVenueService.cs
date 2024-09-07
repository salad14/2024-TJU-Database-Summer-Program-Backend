using VenueBookingSystem.Models;
using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Services
{
    // 定义 IVenueService 接口
    public interface IVenueService
    {

        // 添加新场地的方法签名
        public AddVenueResult AddVenue(VenueDto venueDto);

        // 获取所有不同ID的场地
        IEnumerable<VenueDto> GetAllVenueDetails();

        // 获取指定场地的详细信息
        VenueDetailsDto GetVenueDetails(string venueId);

        // 获取所有维修记录以及相关设备和场地信息
        IEnumerable<RepairDataDto> GetAllRepairRecords();
        // 获取指定设备的详细信息
        EquipmentDetailsDto GetEquipmentDetails(string equipmentId);
        // 获取所有场地信息的方法签名
        IEnumerable<VenueDto> GetAllVenueInfos();

        EditVenueResult EditVenue(string venueId, VenueDto venueDto);

        AddDeviceResult AddDevice(string adminId, string equipmentName, string venueId, DateTime? installationTime);
        EditDeviceResult EditDevice(string equipmentId, string equipmentName, string venueId);
        // 添加维修信息
        AddRepairResult AddRepair(string equipmentId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails);
        // 编辑维修信息
        EditRepairResult EditRepair(string repairId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails);
        // 查询场地的开放时间段
        IEnumerable<AvailabilityDto> GetVenueAvailabilityByDate(string venueId, DateTime date);
        // 修改开放时间段信息
        EditAvailabilityResult EditAvailability(string availabilityId, DateTime startTime, DateTime endTime, decimal price, int remainingCapacity);
        // 添加开放时间段信息
        AddAvailabilityResult AddAvailability(string venueId, DateTime startTime, DateTime endTime, decimal price, int remainingCapacity);
        // 删除开放时间段
        DeleteAvailabilityResult DeleteAvailability(string availabilityId);
        VenueAnnouncementDto GetVenueDetailsAnnouncement(string venueId);

        VenueAdminAndAnnouncementResult GetVenueAdminAndAnnouncements(string venueId);

        AddMaintenanceResult AddMaintenance(string venueId, DateTime maintenanceStartDate, DateTime maintenanceEndDate, string description);

        EditMaintenanceResult EditMaintenance(string maintenanceId, DateTime maintenanceStartDate, DateTime maintenanceEndDate, string description);

        List<EquipmentDetailsDto> GetAllEquipment();

        UpdateVenueAdminResult UpdateVenueAdmin(string venueId, IEnumerable<string> adminArr);
    }
}
