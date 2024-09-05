using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public interface IVenueAvailabilityServices
    {
        //根据时间查询给定时间内开放的所有场地的方法签名
        List<VADto>  GetVenueAvailability(string dateReq);
    }
}
