using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;
using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;
        private readonly IVenueAnalysisService _venueAnalysisService;

        // 构造函数，注入场地服务
        public VenueController(IVenueService venueService, IVenueAnalysisService venueAnalysisService)
        {
            _venueService = venueService;
            _venueAnalysisService = venueAnalysisService;
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
                    info = "An error occurred while retrieving data: " + ex.Message 
                });
            }
        }

        // 获取所有维修记录及其相关设备和场地信息
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
                        info = "场地未找到" 
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
                    info = $"获取场地详情时发生错误：{ex.Message}" 
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

        //获取指定场地类型的分析数据
        [HttpGet("AnalyzeVenueData")]
        public IActionResult AnalyzeVenueData([FromQuery] string venueType)
        {
            try
            {
                var analysisData = _venueAnalysisService.GetVenueAnalysisData(venueType);
                return Ok(new
                {
                    state = 1,
                    data = analysisData,
                    info = ""
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    state = 0,
                    data = (object)null,
                    info = ex.Message
                });
            }
        }

        // 获取指定场地ID的分析数据
        [HttpGet("AnalyzeVenueDataById")]
        public IActionResult AnalyzeVenueDataById([FromQuery] string venueId)
        {
            try
            {
                var analysisData = _venueAnalysisService.GetSingleVenueAnalysisData(venueId);
                return Ok(new
                {
                    state = 1,
                    data = analysisData,
                    info = ""
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    state = 0,
                    data = (object)null,
                    info = ex.Message
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


        // 根据日期查询场地的开放时间段
        [HttpGet("GetVenueAvailabilityByDate")]
        public IActionResult GetVenueAvailabilityByDate([FromQuery] string venueId, [FromQuery] DateTime date)
        {
            try
            {
                var availabilities = _venueService.GetVenueAvailabilityByDate(venueId, date);
                if (availabilities == null || !availabilities.Any())
                {
                    return Ok(new 
                    { 
                        state = 0, 
                        data = (object)null, 
                        info = "未找到该日期的场地开放时间段" 
                    });
                }

                return Ok(new 
                { 
                    state = 1, 
                    data = availabilities, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = $"查询开放时间段时发生错误：{ex.Message}" 
                });
            }
        }


        // 添加保养信息
        [HttpPost("AddMaintenance")]
        public IActionResult AddMaintenance([FromBody] AddMaintenanceRequest request)
        {
            var result = _venueService.AddMaintenance(request.VenueId, request.MaintenanceStartDate, request.MaintenanceEndDate, request.Description);

            return Ok(result);
        }


        // 修改保养信息
        [HttpPut("EditMaintenance")]
        public IActionResult EditMaintenance([FromBody] EditMaintenanceRequest request)
        {
            var result = _venueService.EditMaintenance(request.MaintenanceId, request.MaintenanceStartDate, request.MaintenanceEndDate, request.Description);

            return Ok(result);
        }


        // 修改开放时间段信息
        [HttpPut("EditAvailability")]
        public IActionResult EditAvailability([FromBody] EditAvailabilityRequest request)
        {
            var result = _venueService.EditAvailability(request.AvailabilityId, request.StartTime, request.EndTime, request.Price, request.RemainingCapacity);

            return Ok(result);
        }


        // 添加开放时间段信息
        [HttpPost("AddAvailability")]
        public IActionResult AddAvailability([FromBody] AddAvailabilityRequest request)
        {
            var result = _venueService.AddAvailability(request.VenueId, request.StartTime, request.EndTime, request.Price, request.RemainingCapacity);

            return Ok(result);
        }


         // 删除开放时间段
        [HttpDelete("DeleteAvailability")]
        public IActionResult DeleteAvailability([FromQuery] string availabilityId)
        {
            var result = _venueService.DeleteAvailability(availabilityId);

            return Ok(result);
        }
    }
}