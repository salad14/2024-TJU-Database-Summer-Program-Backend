using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        // 获取所有场地信息
        [HttpGet("GetAllVenues")]
        public IActionResult GetAllVenues()
        {
            var venues = _venueService.GetAllVenues();
            return Ok(venues);
        }

        // 获取所有场地信息
        [HttpGet("GetAllVenueInfos")]
        public IActionResult GetAllVenueInfos()
        {
            try
            {
                var venueInfos = _venueService.GetAllVenueInfos();
                return Ok(new 
                { 
                    state = 1, 
                    data = venueInfos, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving venue information: " + ex.Message 
                });
            }
        }

        // 添加新场地
        [HttpPost("AddVenue")]
        public IActionResult AddVenue(VenueDto venueDto)
        {
            _venueService.AddVenue(venueDto);
            return Ok("场地添加成功");
        }

        // 获取所有不同ID的场地信息
        [HttpGet("GetAllVenueDetails")]
        public IActionResult GetAllVenueDetails()
        {
            try
            {
                var venueDetails = _venueService.GetAllVenueDetails();
                return Ok(new 
                { 
                    state = 1, 
                    data = venueDetails, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving venue details: " + ex.Message 
                });
            }
        }

        // 新增：获取所有维修记录及其相关设备和场地信息
        [HttpGet("GetAllRepairRecords")]
        public IActionResult GetAllRepairRecords()
        {
            try
            {
                var repairRecords = _venueService.GetAllRepairRecords();

                // 成功时返回
                return Ok(new 
                { 
                    state = 1, 
                    data = repairRecords, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving data: " + ex.Message 
                });
            }
        }

        // 获取指定场地的详细信息
        [HttpGet("GetVenueDetails")]
        public IActionResult GetVenueDetails([FromQuery] string venueId)
        {
            try
            {
                var venueDetails = _venueService.GetVenueDetails(venueId);
                if (venueDetails == null)
                {
                    return Ok(new 
                    { 
                        state = 0, 
                        data = (object)null, 
                        info = "Venue not found" 
                    });
                }
                return Ok(new 
                { 
                    state = 1, 
                    data = venueDetails, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving venue details: " + ex.Message 
                });
            }
        }

        // 获取设备的详细信息
        [HttpGet("GetEquipmentDetails")]
        public IActionResult GetEquipmentDetails([FromQuery] string equipmentId)
        {
            try
            {
                var equipmentDetails = _venueService.GetEquipmentDetails(equipmentId);
                if (equipmentDetails == null)
                {
                    return Ok(new 
                    { 
                        state = 0, 
                        data = (object)null, 
                        info = "Equipment not found" 
                    });
                }
                return Ok(new 
                { 
                    state = 1, 
                    data = equipmentDetails, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving equipment details: " + ex.Message 
                });
            }
        }
        // 添加设备信息
        [HttpPost("AddDevice")]
        public IActionResult AddDevice([FromBody] AddDeviceRequest request)
        {
            var result = _venueService.AddDevice(request.AdminId, request.EquipmentName, request.VenueId, request.InstallationTime);

            return Ok(result);
        }
       // 编辑设备信息
        [HttpPut("EditDevice")]
        public IActionResult EditDevice([FromBody] EditDeviceRequest request)
        {
            var result = _venueService.EditDevice(request.EquipmentId, request.EquipmentName, request.VenueId);

            return Ok(result);
        }
         // 添加维修信息
        [HttpPost("AddRepair")]
        public IActionResult AddRepair([FromBody] AddRepairRequest request)
        {
            var result = _venueService.AddRepair(request.EquipmentId, request.MaintenanceStartTime, request.MaintenanceEndTime, request.MaintenanceDetails);

            return Ok(result);
        }
        // 编辑维修信息
        [HttpPut("EditRepair")]
        public IActionResult EditRepair([FromBody] EditRepairRequest request)
        {
            var result = _venueService.EditRepair(request.RepairId, request.MaintenanceStartTime, request.MaintenanceEndTime, request.MaintenanceDetails);

            return Ok(result);
        }

    }   
    public class AddDeviceRequest
    {
        public string AdminId { get; set; } // 管理员ID
        public string EquipmentName { get; set; } // 设备名称
        public string VenueId { get; set; } // 场地ID（可为空）
        public DateTime? InstallationTime { get; set; } // 设备引进时间（可为空）
    } 
    public class EditDeviceRequest
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
        public string VenueId { get; set; } // 场地ID
    }
    public class AddRepairRequest
    {
        public string EquipmentId { get; set; } // 设备ID
        public DateTime MaintenanceStartTime { get; set; } // 维修开始时间
        public DateTime MaintenanceEndTime { get; set; } // 维修结束时间
        public string MaintenanceDetails { get; set; } // 维修描述
    }
    public class EditRepairRequest
    {
        public string RepairId { get; set; } // 维修记录ID
        public DateTime MaintenanceStartTime { get; set; } // 维修开始时间
        public DateTime MaintenanceEndTime { get; set; } // 维修结束时间
        public string MaintenanceDetails { get; set; } // 维修描述
    }
}
