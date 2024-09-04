using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Services
{
    public interface IReservationService
    {
        ReservationResult CreateReservation(ReservationDto reservationDto, string userId);

        ReservationResult CreateGroupReservation(GroupReservationDto groupReservationDto);

        // 新增的方法声明
        bool UpdateReservationStatus(string reservationId, string userId, DateTime checkInTime, string status);

        List<ReservationDetailDto> GetReservationsByVenueIds(List<string> venueIds);

        List<ReservationDetailDto> GetAllReservations();

        List<ReservationDetailDto> GetReservationsByUserId(string userId);

        // 确保返回类型为 List<UserReservationInfoDto>，与实现匹配
        List<UserReservationInfoDto> GetGroupReservationMembers(string reservationId);

    }
}
