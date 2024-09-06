using VenueBookingSystem.Dto;  // 引入 ReservationDto 所在的命名空间
using VenueBookingSystem.Models;
using VenueBookingSystem.Data;  // 引入 IRepository<T> 和 Repository<T> 所在的命名空间
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace VenueBookingSystem.Services
{
    // 实现 IReservationService 接口的 ReservationService 类
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;  // 预约存储库，用于与数据库交互
        private readonly IRepository<User> _userRepository;  // 用户存储库，用于查找和管理用户数据
        private readonly IRepository<Venue> _venueRepository;  // 场地存储库，用于查找和管理场地数据
        private readonly IRepository<UserReservation> _userReservation;
        private readonly IRepository<GroupReservationMember> _groupReservationMemberRepository;
        private readonly IRepository<UserReservation> _userReservationRepository;
        private readonly IRepository<VenueAvailability> _availablityRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly ApplicationDbContext _context;

        // 构造函数，通过依赖注入初始化存储库
        public ReservationService(IRepository<Reservation> reservationRepository,
                                  IRepository<User> userRepository,
                                  IRepository<Venue> venueRepository,
                                  IRepository<UserReservation> userReservation,
                                  IRepository<GroupReservationMember> groupReservationMemberRepository,
                                  IRepository<UserReservation> userReservationRepository,
                                  IRepository<VenueAvailability> availablityRepository,
                                  IRepository<Group> groupRepository,
                                  ApplicationDbContext context)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _userReservation = userReservation;
            _groupReservationMemberRepository = groupReservationMemberRepository;
            _userReservationRepository = userReservationRepository;
            _groupRepository = groupRepository;
            _availablityRepository = availablityRepository;
            _context = context;
        }

        public ReservationResult CreateReservation(ReservationDto reservationDto, string userId)
        {

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = "无效的用户ID,未找到该用户"
                };
            }

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
                ReservationTime = DateTime.Now,
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
                NotificationTime = DateTime.Now
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

        public ReservationResult CreateGroupReservation(GroupReservationDto groupReservationDto)
        {

            // 检查团体是否存在
            var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupReservationDto.GroupId && g.GroupName == groupReservationDto.GroupName);

            if (group == null)
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = "团体信息有误，未找到指定的团体"
                };
            }

            // 检查用户ID是否存在
            var existingUserIds = _context.Users
                .Where(u => groupReservationDto.UserIds.Contains(u.UserId))
                .Select(u => u.UserId)
                .ToList();

            var missingUserIds = groupReservationDto.UserIds.Except(existingUserIds).ToList();

            if (missingUserIds.Any())
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = $"部分用户不存在,缺少的用户ID: {string.Join(", ", missingUserIds)}"
                };
            }
            // 检查时间段的剩余容量
            var availability = _context.VenueAvailabilities.FirstOrDefault(a => a.AvailabilityId == groupReservationDto.AvailabilityId);

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
            if (availability.RemainingCapacity < groupReservationDto.UserIds.Count)
            {
                return new ReservationResult
                {
                    State = 0,
                    ReservationId = null,
                    Info = "剩余容量不足"
                };
            }

            // 减少剩余容量
            availability.RemainingCapacity -= groupReservationDto.UserIds.Count;
            _context.SaveChanges();

            // 生成唯一的预约ID
            var reservationId = GenerateUniqueReservationId();

            // 创建预约记录
            var reservation = new Reservation
            {
                ReservationId = reservationId,
                VenueId = groupReservationDto.VenueId,
                AvailabilityId = groupReservationDto.AvailabilityId,
                ReservationType = groupReservationDto.ReservationType,
                ReservationTime = DateTime.Now,
                PaymentAmount = groupReservationDto.PaymentAmount,
                ReservationItem = groupReservationDto.ReservationItem
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            // 创建团体预约成员记录
            var groupReservationMember = new GroupReservationMember
            {
                ReservationId = reservationId,
                GroupId = groupReservationDto.GroupId
            };
            _context.GroupReservationMembers.Add(groupReservationMember);

            // 创建用户预约关系
            foreach (var userId in groupReservationDto.UserIds)
            {
                var userReservation = new UserReservation
                {
                    UserId = userId,
                    ReservationId = reservationId,
                    NumOfPeople = 1,  // 团体预约人数设置为1
                    Status = "已预约",
                    CheckInTime = null  // 设置签到时间为 null
                };
                _context.UserReservations.Add(userReservation);

                // 生成用户通知
                var notification = new UserNotification
                {
                    UserId = userId,
                    NotificationId = GenerateUniqueNotificationId(),
                    NotificationType = "reservation",
                    Title = "团体预约成功通知",
                    Content = $"您的团体 {groupReservationDto.GroupName} 已成功预约场地 {groupReservationDto.VenueId}，时间段为 {availability.StartTime} - {availability.EndTime}，请按时到场",
                    NotificationTime = DateTime.Now
                };
                _context.UserNotifications.Add(notification);
                _context.SaveChanges();
            }

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
        public ReservationResponseDto GetReservationUser(string reservationId)
        {
            // 从 Reservations 表中获取预约记录
            var reservation = _reservationRepository.GetAll().FirstOrDefault(x => x.ReservationId == reservationId);

            // 检查预约记录是否存在
            if (reservation == null)
            {
                return new ReservationResponseDto
                {
                    State = 0,
                    Info = "预约记录不存在",
                    Data = null
                };
            }

            // 检查预约类型是否为 Group
            if (reservation.ReservationType != "group")
            {
                return new ReservationResponseDto
                {
                    State = 0,
                    Info = "不是团队预约",
                    Data = null
                };
            }

            // 查询团体预约成员信息
            var reservationUsers = _userReservation.GetAll().Where(x => x.ReservationId == reservationId).Select(y =>
                new ReservationUserDetailDto
                {
                    UserId = y.UserId,
                    CheckInTime = y.CheckInTime,
                    Status = y.Status,
                    Username = y.User.Username,
                    RealName = y.User.RealName
                }).ToList();

            return new ReservationResponseDto
            {
                State = 1,
                Info = "",
                Data = reservationUsers
            };
        }


        //预约记录修改
        public void UpdateReservationUser(UpdateReservationUserDto req)
        {
            // 通过预约ID从数据库中获取预约对象
            var userreservation = _userReservation.GetAll().Where(x => x.ReservationId == req.ReservationId && x.UserId == req.UserId).FirstOrDefault();

            // 如果找到对应的预约记录，更新它
            if (userreservation != null)
            {
                userreservation.CheckInTime = req.CheckInTime;
                userreservation.Status = req.Status;
                _userReservation.Update(userreservation);
            }
        }

        //查找所有预约记录
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
                NumOfPeople = _userReservation.GetAll()
                                .Where(t => t.ReservationId == x.ReservationId)
                                .Sum(t => t.NumOfPeople),
                // 仅在 ReservationType 为 "User" 时获取 UserId 和 UserName
                // UserId = x.ReservationType == "individual"
                //     ? _userReservation.GetAll()
                //         .Where(t => t.ReservationId == x.ReservationId)
                //         .Select(t => t.UserId)
                //         .FirstOrDefault()
                //     : null,

                // 通过 UserId 获取 UserName
                // UserName = x.ReservationType == "individual"
                //     ? (from u in _context.Users.AsEnumerable() // 添加 AsEnumerable()
                //        join ur in _userReservation.GetAll().AsEnumerable() // 添加 AsEnumerable()
                //        on u.UserId equals ur.UserId
                //        where ur.ReservationId == x.ReservationId
                //        select u.Username).FirstOrDefault()
                //     : null,

                // 场地名称查询
                VenueName = (from v in _context.Venues.AsEnumerable() // 查询场地名称
                             where v.VenueId == x.VenueId
                             select v.Name).FirstOrDefault(),

                // // 仅在 ReservationType 为 "Group" 时获取团队预约信息，并检查 t.Group 不为空
                // GroupReservationListDto = x.ReservationType == "group"
                //     ? _groupReservationMemberRepository.GetAll()
                //         .Where(t => t.ReservationId == x.ReservationId && t.Group != null)
                //         .Select(t => new GroupReservationListDto
                //         {
                //             GroupId = t.GroupId,
                //             GroupName = t.Group.GroupName
                //         }).ToList()
                //     : null
            }).ToList();
            foreach (var reservation in reservations)
            {
                var availabilityData = _availablityRepository.GetAll()
                .Where(x => x.AvailabilityId == reservation.AvailabilityId)
                .Select(y => new
                {
                    StartTime = y.StartTime,
                    EndTime = y.EndTime
                }).FirstOrDefault();
                reservation.StartTime = availabilityData.StartTime;
                reservation.EndTime = availabilityData.EndTime;
                if (reservation.ReservationType == "individual")
                {
                    var userReservationInfo = _userReservation.GetAll()
                        .Where(t => t.ReservationId == reservation.ReservationId)
                        .Select(t => new
                        {
                            UserId = t.UserId,
                            Status = t.Status,
                        })
                        .FirstOrDefault();
                    reservation.UserId = userReservationInfo.UserId;
                    reservation.Status = userReservationInfo.Status;
                    reservation.UserName = (from u in _context.Users.AsEnumerable()
                                            where u.UserId == userReservationInfo.UserId
                                            select u.Username).FirstOrDefault();
                }
                else if (reservation.ReservationType == "group")
                {
                    reservation.GroupReservationListDto = _groupReservationMemberRepository.GetAll()
                        .Where(t => t.ReservationId == reservation.ReservationId)
                        .Select(t => new GroupReservationListDto
                        {
                            GroupId = t.GroupId,
                            GroupName = _groupRepository.GetAll()
                            .Where(x => x.GroupId == t.GroupId).Select(g => g.GroupName).FirstOrDefault()
                        }).FirstOrDefault();
                }
            }

            return reservations;
        }




        //根据场地ID查找预约记录
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
                NumOfPeople = _userReservation.GetAll().Where(t => t.ReservationId == x.ReservationId).Sum(t => t.NumOfPeople),
            });
            return reservations;
        }

        public bool UpdateReservationStatus(string reservationId, string userId, DateTime checkInTime, string status)
        {
            // 查找用户预约记录
            var userReservation = _context.UserReservations
                .FirstOrDefault(ur => ur.ReservationId == reservationId && ur.UserId == userId);

            if (userReservation == null)
            {
                // 未找到预约记录，返回false
                return false;
            }

            // 更新签到时间和预约状态
            userReservation.CheckInTime = checkInTime;
            userReservation.Status = status;

            // 保存更改
            _context.SaveChanges();

            return true;
        }



        public List<ReservationDetailDto> GetReservationsByUserId(string userId)
        {
            var userReservations = _context.UserReservations
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Reservation)
                .ToList();

            var reservationDetails = new List<ReservationDetailDto>();

            foreach (var userReservation in userReservations)
            {
                var reservation = userReservation.Reservation;
                var reservationAvailability = _availablityRepository.GetAll()
                .Where(a => a.AvailabilityId == reservation.AvailabilityId)
                .Select(x => new
                {
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Price = x.Price,
                }).FirstOrDefault();
                var reservationVenue = _venueRepository.GetAll().Select(x => new
                {
                    VenueId = x.VenueId,
                    Name = x.Name
                }).Where(v => v.VenueId == reservation.VenueId).FirstOrDefault();
                var reservationDetail = new ReservationDetailDto
                {
                    ReservationId = reservation.ReservationId,
                    VenueId = reservation.VenueId,
                    VenueName = reservationVenue.Name,
                    AvailabilityId = reservation.AvailabilityId,
                    StartTime = reservationAvailability.StartTime,
                    EndTime = reservationAvailability.EndTime,
                    ReservationTime = reservation.ReservationTime,
                    PaymentAmount = reservationAvailability.Price,
                    ReservationType = reservation.ReservationType
                };

                if (reservation.ReservationType == "individual")
                {
                    var reservationUserDetail = (from u in _context.Users.AsEnumerable() // 添加 AsEnumerable()
                                                 join ur in _userReservation.GetAll().AsEnumerable() // 添加 AsEnumerable()
                                                 on u.UserId equals ur.UserId
                                                 where ur.ReservationId == reservation.ReservationId
                                                 select new ReservationUserDetailDto
                                                 {
                                                     UserId = u.UserId,
                                                     RealName = u.RealName,
                                                     Status = ur.Status,
                                                     CheckInTime = ur.CheckInTime,
                                                     Username = u.Username,
                                                 }).ToList();
                    reservationDetail.NumOfPeople = userReservation.NumOfPeople; // 返回 NumOfPeople
                    reservationDetail.UserDetails = reservationUserDetail;
                }
                else if (reservation.ReservationType == "group")
                {
                    var groupMemberCount = _context.GroupReservationMembers
                        .Where(gr => gr.ReservationId == reservation.ReservationId)
                        .Count();
                    var groupDetail = (from g in _groupRepository.GetAll().AsEnumerable()
                                       join gr in _groupReservationMemberRepository.GetAll().AsEnumerable()
                                       on g.GroupId equals gr.GroupId
                                       where gr.ReservationId == reservation.ReservationId
                                       select new GroupReservationDetailDto
                                       {
                                           GroupId = g.GroupId,
                                           GroupName = g.GroupName
                                       }).FirstOrDefault();
                    reservationDetail.GroupDetails = groupDetail;
                    reservationDetail.MemberCount = groupMemberCount; // 返回 MemberCount
                }

                reservationDetails.Add(reservationDetail);
            }

            return reservationDetails;
        }

        public List<ReservationDetailDto> GetAllReservations()
        {
            var reservations = _context.Reservations
                .Include(r => r.Venue)
                .Include(r => r.VenueAvailability)
                .ToList();

            var reservationDetails = new List<ReservationDetailDto>();

            foreach (var reservation in reservations)
            {
                var reservationDetail = new ReservationDetailDto
                {
                    ReservationId = reservation.ReservationId,
                    VenueId = reservation.VenueId,
                    VenueName = reservation.Venue?.Name,
                    AvailabilityId = reservation.AvailabilityId,
                    StartTime = reservation.VenueAvailability?.StartTime ?? DateTime.MinValue,
                    EndTime = reservation.VenueAvailability?.EndTime ?? DateTime.MinValue,
                    ReservationTime = reservation.ReservationTime,
                    PaymentAmount = reservation.PaymentAmount,
                    ReservationType = reservation.ReservationType
                };

                if (reservation.ReservationType == "User")
                {
                    var userReservation = _context.UserReservations
                        .FirstOrDefault(ur => ur.ReservationId == reservation.ReservationId);
                    reservationDetail.NumOfPeople = userReservation?.NumOfPeople ?? 0;
                }
                else if (reservation.ReservationType == "Group")
                {
                    var groupReservationCount = _context.GroupReservationMembers
                        .Count(gr => gr.ReservationId == reservation.ReservationId);
                    reservationDetail.MemberCount = groupReservationCount;
                }

                reservationDetails.Add(reservationDetail);
            }

            return reservationDetails;
        }

        public List<ReservationDetailDto> GetReservationsByVenueIds(List<string> venueIds)
        {
            var reservations = _context.Reservations
                .Where(r => venueIds.Contains(r.VenueId))
                .Include(r => r.Venue)
                .Include(r => r.VenueAvailability)
                .ToList();

            var reservationDetails = new List<ReservationDetailDto>();

            foreach (var reservation in reservations)
            {
                var reservationDetail = new ReservationDetailDto
                {
                    ReservationId = reservation.ReservationId,
                    VenueId = reservation.VenueId,
                    VenueName = reservation.Venue?.Name,
                    AvailabilityId = reservation.AvailabilityId,
                    StartTime = reservation.VenueAvailability?.StartTime ?? DateTime.MinValue,
                    EndTime = reservation.VenueAvailability?.EndTime ?? DateTime.MinValue,
                    ReservationTime = reservation.ReservationTime,
                    PaymentAmount = reservation.PaymentAmount,
                    ReservationType = reservation.ReservationType
                };

                if (reservation.ReservationType == "User")
                {
                    var userReservation = _context.UserReservations
                        .FirstOrDefault(ur => ur.ReservationId == reservation.ReservationId);
                    reservationDetail.NumOfPeople = userReservation?.NumOfPeople ?? 0;
                }
                else if (reservation.ReservationType == "Group")
                {
                    var groupReservationCount = _context.GroupReservationMembers
                        .Count(gr => gr.ReservationId == reservation.ReservationId);
                    reservationDetail.MemberCount = groupReservationCount;
                }

                reservationDetails.Add(reservationDetail);
            }

            return reservationDetails;
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
