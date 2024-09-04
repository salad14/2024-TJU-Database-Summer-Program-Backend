using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public interface IVenueAvailabilityServices
    {
        List<VADto>  GetVenueAvailability(string dateReq);
    }
}
