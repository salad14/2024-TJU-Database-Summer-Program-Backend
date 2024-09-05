using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sports_management.Services;
using VenueBookingSystem.Models;
using VenueBookingSystem.Services;

namespace sports_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class VenueAvailabilityController : ControllerBase
    {
        private readonly IVenueAvailabilityServices _venueAvailabilityService;
        public VenueAvailabilityController(IVenueAvailabilityServices venueAvailabilityService)
        {
            _venueAvailabilityService = venueAvailabilityService;
        }

        //根据所给时间查询给定时间内开放的所有场地
        [HttpGet("GetVenueAvailability")]
        public IActionResult GetVenueAvailability([FromQuery] string dateReq)
        {
            var group = _venueAvailabilityService.GetVenueAvailability(dateReq);
            if (group == null)
            {
                return NotFound("未找到符合要求的场地");
            }
            return Ok(group); 
        }
    }
}
