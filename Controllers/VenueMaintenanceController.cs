using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using sports_management.Dto;
using sports_management.Services;
using VenueBookingSystem.Data;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace sports_management.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    public class VenueMaintenanceController : Controller
    {
        private readonly IVenueMaintenanceService _venueMaintenanceService;

        public VenueMaintenanceController(IVenueMaintenanceService venueMaintenanceService)
        {
            _venueMaintenanceService = venueMaintenanceService;
        }

        // 获取所有保养信息
        [HttpGet]
        public IActionResult GetAllVenueMaintenances()
        {
            var venueMaintenances= _venueMaintenanceService.GetAllVenueMaintenances();
            return Ok(venueMaintenances);
        }
        // 获取某个场地的保养信息
        [HttpGet]
        public IActionResult GetAllVenueMaintenancesByVenueId(string Id)
        {
            var venueMaintenances = _venueMaintenanceService.GetAllVenueMaintenancesByVenueId(Id);
            return Ok(venueMaintenances);
        }

        // 添加保养
        [HttpPost]
        public IActionResult AddMaintenance(VenueMaintenanceDto venueMaintenanceDto)
        {
            try
            {
                var r = _venueMaintenanceService.AddVenueMaintenance(venueMaintenanceDto);
                if (r != null)
                {
                    return Ok(new { state = 1, RepairId = r.VenueMaintenanceId, info = "" });
                }
                else
                {
                    return Ok(new { state = 0, RepairId = "", info = "添加失败" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { state = 0, RepairId = "", info = "添加失败" });
            }
            
        }         
        // 更改保养          
        [HttpPost]
        public IActionResult UpdateVenueMaintenance(VenueMaintenanceDto venueMaintenanceDto)
        {
            try
            {
                _venueMaintenanceService.UpdateVenueMaintenance(venueMaintenanceDto);
                return Ok(new { state = 1, RepairId = venueMaintenanceDto.VenueMaintenanceId, info = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { state = 0, RepairId = "", info = "修改失败" });
            }

        }
    }
}
