using sports_management.Dto;
using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public interface IVenueMaintenanceService
    {

        // 获取所有场地的方法签名
        IEnumerable<VenueMaintenance> GetAllVenueMaintenances();

        // 添加新场地的方法签名
        VenueMaintenance AddVenueMaintenance(VenueMaintenanceDto venueMaintenance);


        void UpdateVenueMaintenance(VenueMaintenanceDto venueMaintenance);

        IEnumerable<VenueMaintenance> GetAllVenueMaintenancesByVenueId(string VenueId);

        VenueMaintenance GetAllVenueMaintenancesByMaintenanceId(string MaintenanceId);

    }
}
