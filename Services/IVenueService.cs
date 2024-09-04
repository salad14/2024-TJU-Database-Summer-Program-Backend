using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IVenueService
    {
        // 获取所有场地的方法签名
        IEnumerable<Venue> GetAllVenues();

        // 添加新场地的方法签名
        void AddVenue(VenueDto venueDto);

        // 获取所有不同ID的场地
        IEnumerable<VenueDto> GetAllVenueDetails();
        
        // 获取指定场地的详细信息
        VenueDetailsDto GetVenueDetails(string venueId);

        // 获取所有维修记录以及相关设备和场地信息
        IEnumerable<RepairDataDto> GetAllRepairRecords();
        // 获取设备详情信息
        EquipmentDetailsDto GetEquipmentDetails(string equipmentId);
        // 获取所有场地信息的方法签名
        IEnumerable<VenueDto> GetAllVenueInfos();
        AddDeviceResult AddDevice(string adminId, string equipmentName, string venueId, DateTime? installationTime);
        EditDeviceResult EditDevice(string equipmentId, string equipmentName, string venueId);
        // 添加维修信息
        AddRepairResult AddRepair(string equipmentId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails);
        // 编辑维修信息
        EditRepairResult EditRepair(string repairId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails);
    }
}
