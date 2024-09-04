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

        // 获取预约人信息的方法签名
        IEnumerable<ReservationDetailDto> GetReservationUser(string reservationId);

        // 更新预约用户的方法签名
        void UpdateReservationUser(UpdateReservationUserDto req);

        // 获取所有预约记录的方法签名
        IEnumerable<ReservationListDto> GetReservationList();

        // 根据场地ID查找预约记录的方法签名
        IEnumerable<ReservationVenueListDto> GetReservationVenueList(string venueId);
    }

    // 实现 IReservationService 接口的 ReservationService 类
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;  // 预约存储库，用于与数据库交互
        private readonly IRepository<User> _userRepository;  // 用户存储库，用于查找和管理用户数据
        private readonly IRepository<Venue> _venueRepository;  // 场地存储库，用于查找和管理场地数据
        private readonly IRepository<UserReservation> _userReservation; // 场地存储库，用于查找和管理场地数据
        private readonly IRepository<GroupReservationMember> _groupReservationMemberRepository;

        // 构造函数，通过依赖注入初始化存储库
        public ReservationService(IRepository<Reservation> reservationRepository,
                                  IRepository<User> userRepository,
                                  IRepository<Venue> venueRepository,
                                  IRepository<UserReservation> userReservation,
                                  IRepository<GroupReservationMember> groupReservationMemberRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _userReservation = userReservation;
            _groupReservationMemberRepository = groupReservationMemberRepository;
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
                StartTime = reservationDto.StartTime,  // 设置预约开始时间
                EndTime = reservationDto.EndTime,  // 设置预约结束时间
                PaymentAmount = reservationDto.PaymentAmount,  // 设置支付金额
                VenueId = reservationDto.VenueId,  // 设置关联的场地ID
                AvailabilityId = reservationDto.AvailabilityId,  // 设置关联的开放时间段ID
                ReservationItem = reservationDto.ReservationItem,  // 设置预约项目描述
                ReservationTime = DateTime.UtcNow,  // 设置预约操作时间
                ReservationType = reservationDto.ReservationType,  // 设置预约类型
                NumOfPeople = reservationDto.NumOfPeople,  // 设置预约人数
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

        // 获取预约人的信息
        public IEnumerable<ReservationDetailDto> GetReservationUser(string reservationId)
        {
            var reservation = _userReservation.GetAll().Where(x => x.ReservationId == reservationId).Select(y =>
                new ReservationDetailDto
                {
                    UserId = y.UserId,
                    CheckInTime = y.CheckInTime,
                    Status = y.Status,
                    Username = y.User.Username,
                    RealName = y.User.RealName
                });

            return reservation;
        }

        // 更新预约用户
        public void UpdateReservationUser(UpdateReservationUserDto req)
        {
            // 通过预约ID从数据库中获取预约对象
            var userreservation = _userReservation.GetAll().Where(x => x.ReservationId == req.ReservationId && x.UserId == req.UserId).FirstOrDefault();

            // 如果找到对应的预约记录，删除它
            if (userreservation != null)
            {
                userreservation.CheckInTime = req.CheckInTime;
                userreservation.Status = req.Status;
                _userReservation.Update(userreservation);
            }
        }

        // 查找所有预约记录
        public IEnumerable<ReservationListDto> GetReservationList()
        {
            var reservations = _reservationRepository.GetAll().Select(x => new ReservationListDto
            {
                ReservationId = x.ReservationId,
                VenueId = x.VenueId,
                AvailabilityId = x.AvailabilityId,
                ReservationType = x.ReservationType,
                ReservationTime = x.ReservationTime,
                PaymentAmount = x.PaymentAmount,
                StartTime = x.Availability.StartTime,
                EndTime = x.Availability.EndTime,
                NumOfPeople = _userReservation.GetAll().Where(t => t.ReservationId == x.ReservationId).Sum(t => t.NumOfPeople),
                GroupReservationListDto = _groupReservationMemberRepository.GetAll().Where(t => t.ReservationId != x.ReservationId).Select(t => new GroupReservationListDto
                {
                    GroupId = t.GroupId,
                    GroupName = t.Group.GroupName
                }).ToList(),
            });
            return reservations;
        }

        // 根据场地ID查找预约记录
        public IEnumerable<ReservationVenueListDto> GetReservationVenueList(string venueId)
        {
            var reservations = _reservationRepository.GetAll().Where(x => x.VenueId == venueId).Select(x => new ReservationVenueListDto
            {
                ReservationId = x.ReservationId,
                VenueId = x.VenueId,
                AvailabilityId = x.AvailabilityId,
                ReservationType = x.ReservationType,
                ReservationTime = x.ReservationTime,
                PaymentAmount = x.PaymentAmount,
                StartTime = x.Availability.StartTime,
                EndTime = x.Availability.EndTime,
                NumOfPeople = _userReservation.GetAll().Where(t => t.ReservationId == x.ReservationId).Sum(t => t.NumOfPeople),
            });
            return reservations;
        }
    }
}
