using VenueBookingSystem.Dto;  // 引入 ReservationDto 所在的命名空间
using VenueBookingSystem.Models;
using VenueBookingSystem.Data;  // 引入 IRepository<T> 和 Repository<T> 所在的命名空间

namespace VenueBookingSystem.Services
{
    // 定义 IReservationService 接口
    public interface IReservationService
    {
        // 创建预约的方法签名
        void CreateReservation(ReservationDto reservationDto);

        // 取消预约的方法签名
        void CancelReservation(int reservationId);

        //预约记录界面查询成员预约信息
        List<UserReservationInfoDto> GetGroupReservationMembers(string reservationId);
    }

    // 实现 IReservationService 接口的 ReservationService 类
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;  // 预约存储库，用于与数据库交互
        private readonly IRepository<User> _userRepository;  // 用户存储库，用于查找和管理用户数据
        private readonly IRepository<Venue> _venueRepository;  // 场地存储库，用于查找和管理场地数据
        private readonly IRepository<UserReservation> _userReservationRepository;

        // 构造函数，通过依赖注入初始化存储库
        public ReservationService(IRepository<Reservation> reservationRepository,
                                  IRepository<User> userRepository,
                                   IRepository<UserReservation> userReservationRepository,
                                  IRepository<Venue> venueRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _userReservationRepository = userReservationRepository;
        }

        // 创建预约
        public void CreateReservation(ReservationDto reservationDto)
        {
            // 从数据库中获取与预约相关的 User 和 Venue 对象
            var user = _userRepository.GetById(reservationDto.UserId);
            var venue = _venueRepository.GetById(reservationDto.VenueId);

            // 如果未找到对应的用户或场地，抛出异常
            if (user == null || venue == null)
            {
                throw new ArgumentException("用户或场地未找到");
            }

            // 创建新的预约对象，并设置其属性
            var reservation = new Reservation
            {
                ReservationId = Guid.NewGuid().ToString(), // 自动生成 ReservationId
                PaymentAmount = reservationDto.PaymentAmount,  // 设置支付金额
                VenueId = reservationDto.VenueId,  // 设置关联的场地ID
                AvailabilityId = reservationDto.AvailabilityId,  // 设置关联的开放时间段ID
                ReservationItem = reservationDto.ReservationItem,  // 设置预约项目描述
                ReservationTime = DateTime.UtcNow,  // 设置预约操作时间
                ReservationType =reservationDto.ReservationType,
                Venue = venue
            };

            // 将新的预约记录添加到数据库
            _reservationRepository.Add(reservation);
        }

        // 取消预约
        public void CancelReservation(int reservationId)
        {
            // 通过预约ID从数据库中获取预约对象
            var reservation = _reservationRepository.GetById(reservationId);
            
            // 如果找到对应的预约记录，删除它
            if (reservation != null)
            {
                _reservationRepository.Delete(reservation);
            }
        }

        // 查找团体预约每个成员的预约信息
        public List<UserReservationInfoDto> GetGroupReservationMembers(string reservationId)
        {
            // 获取用户预约关系表中的记录
            var userReservations = _userReservationRepository
                .Find(ur => ur.ReservationId == reservationId).ToList();

            // 创建结果列表
            var memberDetails = new List<UserReservationInfoDto>();

            foreach (var userReservation in userReservations)
            {
                // 查找每个用户的详细信息
                var user = _userRepository.GetById(userReservation.UserId);

                // 将结果加入到列表
                memberDetails.Add(new UserReservationInfoDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    RealName = user.RealName,
                    CheckInTime = userReservation.CheckInTime,
                    Status = userReservation.Status
                });
            }

            return memberDetails;
        }
    }
}
