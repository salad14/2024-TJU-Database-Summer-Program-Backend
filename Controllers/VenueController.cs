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

        // 构造函数，注入场地服务
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

        // 新增：获取所有不同ID的场地信息
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
        // 获取指定设备的详细信息
        [HttpGet("GetDeviceDetails")]
        public IActionResult GetDeviceDetails([FromQuery] string equipmentId)
        {
            try
            {
                var deviceDetails = _venueService.GetDeviceDetails(equipmentId);
                if (deviceDetails == null)
                {
                    return Ok(new 
                    { 
                        state = 0, 
                        data = (object)null, 
                        info = "Device not found" 
                    });
                }
                return Ok(new 
                { 
                    state = 1, 
                    data = deviceDetails, 
                    info = "" 
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    state = 0, 
                    data = (object)null, 
                    info = "An error occurred while retrieving device details: " + ex.Message 
                });
            }
        }
    }
}
