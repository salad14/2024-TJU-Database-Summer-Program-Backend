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


        public VenueMaintenance AddVenueMaintenance(VenueMaintenanceDto venueMaintenanceDto)
        {
            var venueMaintenance1 = new VenueMaintenance
            {
                VenueMaintenanceId = GenerateVenueMaintenanceId(),
                Description = venueMaintenanceDto.Description,
                MaintenanceStartDate = venueMaintenanceDto.MaintenanceStartDate,
                MaintenanceEndDate = venueMaintenanceDto.MaintenanceEndDate,
                VenueId = venueMaintenanceDto.VenueId
            };
            _venueMaintenanceRepository.Add(venueMaintenance1);
            return venueMaintenance1;
        }

        private string GenerateVenueMaintenanceId()
        {
            // 获取当前数据库中最大的 VenueMaintenanceId
            var maxVenueMaintenanceId = _venueMaintenanceRepository.GetAll()
                .Select(v => Convert.ToInt32(v.VenueMaintenanceId))
                .DefaultIfEmpty(0) // 如果没有记录，则返回0
                .Max();

            // 生成下一个 VenueMaintenanceId
            int newVenueMaintenanceId = maxVenueMaintenanceId + 1;

            // 返回新的 VenueMaintenanceId，转换为字符串
            return newVenueMaintenanceId.ToString();
        }


        public IEnumerable<VenueMaintenance> GetAllVenueMaintenances()
        {
            return _venueMaintenanceRepository.GetAll();
        }

        public VenueMaintenance GetAllVenueMaintenancesByMaintenanceId(string MaintenanceId)
        {
            return _venueMaintenanceRepository.Find(g => g.VenueMaintenanceId == MaintenanceId)?.FirstOrDefault();
        }

        public IEnumerable<VenueMaintenance> GetAllVenueMaintenancesByVenueId(string VenueId)
        {
            return _venueMaintenanceRepository.GetAll().Where(x => x.VenueId == VenueId);
        }

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
