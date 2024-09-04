using VenueBookingSystem.Dto;  // 引入 ReservationDto 所在的命名空间
using VenueBookingSystem.Models;
using VenueBookingSystem.Data;  // 引入 IRepository<T> 和 Repository<T> 所在的命名空间

namespace VenueBookingSystem.Services
{
    // 实现 IReservationService 接口的 ReservationService 类
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;  // 预约存储库，用于与数据库交互
        private readonly IRepository<User> _userRepository;  // 用户存储库，用于查找和管理用户数据
        private readonly IRepository<Venue> _venueRepository;  // 场地存储库，用于查找和管理场地数据
        private readonly ApplicationDbContext _context;

        // 构造函数，通过依赖注入初始化存储库
        public ReservationService(IRepository<Reservation> reservationRepository,
                                  IRepository<User> userRepository,
                                  IRepository<Venue> venueRepository,
                                  ApplicationDbContext context)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _context = context;
        }

        public ReservationResult CreateReservation(ReservationDto reservationDto, string userId)
        {
            // 检查时间段的剩余容量
            var availability = _context.VenueAvailabilities.FirstOrDefault(a => a.AvailabilityId == reservationDto.AvailabilityId);

            if (availability == null)
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = "未找到指定的开放时间段"
                };
            }

            // 检查剩余容量
            if (availability.RemainingCapacity < reservationDto.NumOfPeople)
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = "剩余容量不足"
                };
            }

            // 减少剩余容量
            availability.RemainingCapacity -= reservationDto.NumOfPeople;
            _context.SaveChanges();

            // 生成唯一的预约ID
            var reservationId = GenerateUniqueReservationId();

            // 创建预约记录
            var reservation = new Reservation
            {
                ReservationId = reservationId,
                VenueId = reservationDto.VenueId,
                AvailabilityId = reservationDto.AvailabilityId,
                ReservationType = reservationDto.ReservationType,
                ReservationTime = DateTime.UtcNow,
                PaymentAmount = reservationDto.PaymentAmount,
                ReservationItem = reservationDto.ReservationItem
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            // 创建用户预约关系
            var userReservation = new UserReservation
            {
                UserId = userId,
                ReservationId = reservationId,
                NumOfPeople = reservationDto.NumOfPeople,
                Status = "已预约",
                CheckInTime = null
            };

            _context.UserReservations.Add(userReservation);
            _context.SaveChanges();

            // 生成用户通知
            var notification = new UserNotification
            {
                UserId = userId,
                NotificationId = GenerateUniqueNotificationId(),
                NotificationType = "reservation",
                Title = "预约成功通知",
                Content = $"您已成功预约场地 {reservationDto.VenueId}，时间段为 {availability.StartTime} - {availability.EndTime}，请按时到场",
                NotificationTime = DateTime.UtcNow
            };

            _context.UserNotifications.Add(notification);
            _context.SaveChanges();

            return new ReservationResult
            {
                State = 1,
                ReservationId = reservationId,
                Info = ""
            };
        }


        private string GenerateUniqueReservationId()
        {
            // 获取当前最大的预约ID，如果没有预约记录，则从100001开始
            var maxId = _context.Reservations.Max(r => (int?)Convert.ToInt32(r.ReservationId)) ?? 100000;
            return (maxId + 1).ToString();
        }

        private string GenerateUniqueNotificationId()
        {
            // 获取当前最大的通知ID，如果没有通知记录，则从100001开始
            var maxId = _context.UserNotifications.Max(n => (int?)Convert.ToInt32(n.NotificationId)) ?? 100000;
            return (maxId + 1).ToString();
        }
    }
}
