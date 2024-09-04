using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Services
{
    public interface IReservationService
    {
        ReservationResult CreateReservation(ReservationDto reservationDto, string userId);
    }
}
