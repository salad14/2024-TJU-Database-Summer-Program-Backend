using VenueBookingSystem.Dto;

namespace VenueBookingSystem.Services
{
    public interface IReservationService
    {
        ReservationResult CreateReservation(ReservationDto reservationDto, string userId);

        ReservationResult CreateGroupReservation(GroupReservationDto groupReservationDto);

        //获取预约人信息的方法签名
        IEnumerable<ReservationDetailDto> GetReservationUser(string reservationId);

        //更新预约用户的方法签名
        void UpdateReservationUser(UpdateReservationUserDto req);

        //获取所有预约记录的方法签名
        IEnumerable<ReservationListDto> GetReservationList();

        //根据场地ID查找预约记录的方法签名
        IEnumerable<ReservationVenueListDto> GetReservationVenueList(string venueId);
    }
}
