using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public class UserReservationService : IUserReservationService
    {
        private readonly IRepository<UserReservation> _userReservationRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<User> _userRepository;

        public UserReservationService(
            IRepository<UserReservation> userReservationRepository, 
            IRepository<Reservation> reservationRepository, 
            IRepository<User> userRepository)
        {
            _userReservationRepository = userReservationRepository;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
        }
        
        // 更新用户违约状态
        public void UpdateUserViolation(string userId)
        {
            var userReservation = _userReservationRepository.Find(x => x.UserId == userId).ToList();
            if (userReservation.Any())
            {
                var time = DateTime.Now;
                foreach (var item in userReservation)
                {
                    // 获取与用户预约相关的预约记录的结束时间
                    var endtime = _reservationRepository.Find(x => x.ReservationId == item.ReservationId).FirstOrDefault()?.Availability?.EndTime;
                    // 判断当前时间是否超过预约结束时间
                    if (time > endtime && (item.Status != "已签到" || item.Status != "已取消"))
                    {
                        item.Status = "已违约";
                        _userReservationRepository.Update(item);

                        var user = _userRepository.Find(x => x.UserId == userId).FirstOrDefault();
                        if (user != null)
                        {
                            user.ViolationCount += 1; 
                            _userRepository.Update(user);
                        }
                    }
                }
            }
        }
    }
}
