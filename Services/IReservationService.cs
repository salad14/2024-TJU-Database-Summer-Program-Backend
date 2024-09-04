using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Services
{
    public interface IReservationService
    {
        ReservationResult CreateReservation(ReservationDto reservationDto, string userId);

        ReservationResult CreateGroupReservation(GroupReservationDto groupReservationDto);

        // �����ķ�������
        bool UpdateReservationStatus(string reservationId, string userId, DateTime checkInTime, string status);

        List<ReservationDetailDto> GetReservationsByVenueIds(List<string> venueIds);

        List<ReservationDetailDto> GetAllReservations();

        List<ReservationDetailDto> GetReservationsByUserId(string userId);

        // ȷ����������Ϊ List<UserReservationInfoDto>����ʵ��ƥ��
        List<UserReservationInfoDto> GetGroupReservationMembers(string reservationId);

    }
}
