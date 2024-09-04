using sports_management.Dto;
using System.Text.RegularExpressions;
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public class VenueMaintenanceService : IVenueMaintenanceService
    {
        private readonly IRepository<VenueMaintenance> _venueMaintenanceRepository;

        public VenueMaintenanceService(IRepository<VenueMaintenance> venueMaintenanceRepository)
        {
            _venueMaintenanceRepository = venueMaintenanceRepository;
        }

        // 添加新的场地保养记录
        public VenueMaintenance AddVenueMaintenance(VenueMaintenanceDto venueMaintenanceDto)
        {
            var venueMaintenance1 = new VenueMaintenance
            {
                VenueMaintenanceId = Guid.NewGuid().ToString(), // 使用 Guid 生成唯一的 ID
                Description = venueMaintenanceDto.Description,
                MaintenanceStartDate = venueMaintenanceDto.MaintenanceStartDate,
                MaintenanceEndDate = venueMaintenanceDto.MaintenanceEndDate,
                VenueId = venueMaintenanceDto.VenueId
            };
            _venueMaintenanceRepository.Add(venueMaintenance1);
            return venueMaintenance1;
        }

        // 获取所有的场地保养记录
        public IEnumerable<VenueMaintenance> GetAllVenueMaintenances()
        {
            return _venueMaintenanceRepository.GetAll();
        }

        // 通过保养ID获取所有场地保养记录
        public VenueMaintenance GetAllVenueMaintenancesByMaintenanceId(string MaintenanceId)
        {
            return _venueMaintenanceRepository.Find(g => g.VenueMaintenanceId == MaintenanceId)?.FirstOrDefault();
        }

        // 通过场地ID获取所有相关的场地保养记录
        public IEnumerable<VenueMaintenance> GetAllVenueMaintenancesByVenueId(string VenueId)
        {
            return _venueMaintenanceRepository.GetAll().Where(x => x.VenueId == VenueId);
        }

        // 更新已有的场地保养记录
        public void UpdateVenueMaintenance(VenueMaintenanceDto venueMaintenanceDto)
        {
            var venueMaintenance1 = new VenueMaintenance
            {
                VenueMaintenanceId = venueMaintenanceDto.VenueMaintenanceId,
                Description = venueMaintenanceDto.Description,
                MaintenanceStartDate = venueMaintenanceDto.MaintenanceStartDate,
                MaintenanceEndDate = venueMaintenanceDto.MaintenanceEndDate,
                VenueId = venueMaintenanceDto.VenueId
            };
            _venueMaintenanceRepository.Update(venueMaintenance1);
        }
    }
}
