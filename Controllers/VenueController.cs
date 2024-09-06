using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Data;

namespace VenueBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;
        private readonly IVenueAnalysisService _venueAnalysisService;
        private readonly ApplicationDbContext _context;

        // 构造函数，注入场地服务
        public VenueController(IVenueService venueService, IVenueAnalysisService venueAnalysisService,ApplicationDbContext context)
        {
            _venueService = venueService;
            _venueAnalysisService = venueAnalysisService;
            _context = context;
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
        public IActionResult AddVenue([FromBody] VenueDto venueDto)
        {
            try
            {
                // 调用服务层的方法添加场地，并返回分配的场地ID
                var result = _venueService.AddVenue(venueDto);
                
                // 如果添加成功
                if (!string.IsNullOrEmpty(result.VenueId))
                {
                    return Ok(new 
                    { 
                        state = 1, 
                        info = "", 
                        venueId = result.VenueId  // 返回分配的场地ID
                    });
                }
                // 如果场地名称已存在
                if (result.Info == "场地名称已存在")
                {
                    return Ok(new 
                    { 
                        state = 0, 
                        info = "场地名称已存在", 
                        venueId = (object)null
                    });
                }

                // 添加失败时的处理
                return Ok(new 
                { 
                    state = 0, 
                    info = "场地添加失败", 
                    venueId = (object)null
                });
            }
            catch (Exception ex)
            {
                // 处理异常情况
                return Ok(new 
                { 
                    state = 0, 
                    info = $"添加场地时发生错误：{ex.Message}", 
                    venueId = (object)null
                });
            }
        }

        //编辑场地信息
        [HttpPut("EditVenue")]
        public IActionResult EditVenue([FromQuery] string venueId, [FromBody] VenueDto venueDto)
        {
            try
            {
                var result = _venueService.EditVenue(venueId, venueDto);
                if (result.State == 1)
                {
                    return Ok(new
                    {
                        state = result.State,
                        info = result.Info
                    });
                }
                else
                {
                    return Ok(new
                    {
                        state = result.State,
                        info = result.Info
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    state = 0,
                    info = $"编辑场地信息时发生错误：{ex.Message}"
                });
            }
        }

        //获取场地管理员和公告信息
        [HttpGet("GetVenueAdminAndAnnouncements")]
        public IActionResult GetVenueAdminAndAnnouncements([FromQuery] string venueId)
        {
            try
            {
                var result = _venueService.GetVenueAdminAndAnnouncements(venueId);
                if (result.State == 1)
                {
                    return Ok(new
                    {
                        state = result.State,
                        info = result.Info,
                        data = result.Data
                    });
                }
                else
                {
                    return Ok(new
                    {
                        state = result.State,
                        info = result.Info
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    state = 0,
                    info = $"获取场地管理员和公告详情时发生错误：{ex.Message}"
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
        [HttpPost("DeleteAvailability")]
        public IActionResult DeleteAvailability([FromBody] DeleteAvailabilityRequest request)
        {
            var result = _venueService.DeleteAvailability(request.AvailabilityId);

            return Ok(result);
        }



        //根据场地ID获取场地公告
        [HttpGet("GetVenueDetailsAnnouncement")]
        public IActionResult GetVenueDetailsAnnouncement([FromQuery] string venueId)
        {
            try
            {
                var venueDetails = _venueService.GetVenueDetailsAnnouncement(venueId);
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

        // 添加保养信息
        [HttpPost("AddMaintenance")]
        public IActionResult AddMaintenance([FromBody] VenueMaintenanceDto venueMaintenanceDto)
        {
            try
            {
                // 调用服务层，生成保养记录
                var result = _venueService.AddMaintenance(
                    venueMaintenanceDto.VenueId, 
                    venueMaintenanceDto.MaintenanceStartDate, 
                    venueMaintenanceDto.MaintenanceEndDate, 
                    venueMaintenanceDto.Description
                );

                return Ok(new { state = result.State, VenueMaintenanceId = result.MaintenanceId, info = result.Info });
            }
            catch (Exception ex)
            {
                return BadRequest(new { state = 0, VenueMaintenanceId = "", info = $"添加失败: {ex.Message}" });
            }
        }


        // 获取所有保养信息
        [HttpGet("GetAllVenueMaintenances")]
        public IActionResult GetAllVenueMaintenances()
        {
            try
            {
                // 从 VenueMaintenances 表中获取所有保养信息
                var venueMaintenances = _context.VenueMaintenances
                    .Select(vm => new VenueMaintenanceDto
                    {
                        VenueMaintenanceId = vm.VenueMaintenanceId,
                        VenueId = vm.VenueId,
                        MaintenanceStartDate = vm.MaintenanceStartDate,
                        MaintenanceEndDate = vm.MaintenanceEndDate,
                        Description = vm.Description
                    }).ToList();

                return Ok(new { state = 1, data = venueMaintenances, info = "" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { state = 0, info = $"获取失败: {ex.Message}" });
            }
        }

        // 获取某个场地的保养信息
        [HttpGet("GetAllVenueMaintenancesByVenueId")]
        public IActionResult GetAllVenueMaintenancesByVenueId(string venueId)
        {
            try
            {
                // 检查场地是否存在
                var venueExists = _context.Venues
                    .Where(v => v.VenueId == venueId)
                    .Select(v => 1) 
                    .FirstOrDefault() == 1;
                if (!venueExists)
                {
                    return NotFound(new { state = 0, info = "未找到该场地" });
                }

                // 根据场地 ID 获取该场地的保养记录
                var venueMaintenances = _context.VenueMaintenances
                    .Where(vm => vm.VenueId == venueId)
                    .Select(vm => new VenueMaintenanceDto
                    {
                        VenueMaintenanceId = vm.VenueMaintenanceId,
                        VenueId = vm.VenueId,
                        MaintenanceStartDate = vm.MaintenanceStartDate,
                        MaintenanceEndDate = vm.MaintenanceEndDate,
                        Description = vm.Description
                    }).ToList();

                // 返回保养记录
                return Ok(new { state = 1, data = venueMaintenances, info = "" });
            }
            catch (Exception ex)
            {
                // 返回错误信息
                return BadRequest(new { state = 0, info = $"获取失败: {ex.Message}" });
            }
        }



        // 更新保养信息
        [HttpPut("UpdateVenueMaintenance")]
        public IActionResult UpdateVenueMaintenance([FromBody] VenueMaintenanceDto venueMaintenanceDto)
        {
            try
            {
                // 调用服务层，传递更新的保养记录
                var result = _venueService.EditMaintenance(
                    venueMaintenanceDto.VenueMaintenanceId,
                    venueMaintenanceDto.MaintenanceStartDate,
                    venueMaintenanceDto.MaintenanceEndDate,
                    venueMaintenanceDto.Description
                );

                // 根据服务层的返回值决定返回给前端的状态
                if (result.State == 1)
                {
                    return Ok(new { state = 1, info = "保养信息更新成功" });
                }
                else
                {
                    return BadRequest(new { state = 0, info = result.Info });
                }
            }
            catch (Exception ex)
            {
                // 捕获异常并返回详细错误信息
                return BadRequest(new { state = 0, info = $"更新失败: {ex.Message}" });
            }
        }



    }
}