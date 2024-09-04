using Microsoft.EntityFrameworkCore;
using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<AdminNotification> _adminNotificationRepository;
        private readonly ApplicationDbContext _context;

        public AdminService(IRepository<Admin> adminRepository, IRepository<Announcement> announcementRepository,IRepository<AdminNotification> adminNotificationRepository,ApplicationDbContext context)
        {
            _adminRepository = adminRepository;
            _announcementRepository = announcementRepository;
            _adminNotificationRepository = adminNotificationRepository;
            _context = context;
        }

        public Admin GetAdminById(string adminId)
        {
            return _adminRepository.Find(a => a.AdminId == adminId).FirstOrDefault();
        }

        public void CreateAdmin(Admin admin)
        {
            _adminRepository.Add(admin);
        }

        public IEnumerable<object> GetAdminNoticeData(string adminId)
        {
            // 首先验证adminId是否存在
            var admin = _adminRepository
            .Find(a => a.AdminId == adminId)
            .Select(a => new AdminRequestDto
            {
                AdminId = a.AdminId,
                ContactNumber = a.ContactNumber,
                Password = a.Password
            });

            if (admin == null)
            {
                throw new ArgumentException("无效的管理员ID");
            }

            // 从 AdminNotifications 表中筛选与 AdminId 相关的通知
            var notifications = _context.AdminNotifications
            .FromSqlRaw("SELECT * FROM \"AdminNotifications\" WHERE \"AdminId\" = {0}", adminId)
            .Select(n => new AdminNotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationType = n.NotificationType,
                Title = n.Title,
                Content = n.Content,
                NotificationTime = n.NotificationTime,
                NewAdminId = n.NewAdminId
            })
            .ToList();

            if (notifications == null)
            {
                return Enumerable.Empty<object>(); // 如果没有找到通知，返回一个空的集合
            }

            return notifications;
        }

        public AdminManagedItemsResultDto GetAdminManagedVenuesAndEquipment(string adminId)
        {
            try
            {
                // 验证管理员ID是否存在
                var admin = _adminRepository.Find(a => a.AdminId == adminId).FirstOrDefault();
                if (admin == null)
                {
                    return new AdminManagedItemsResultDto
                    {
                        State = 0,
                        Info = "无效的管理员ID",
                        Data = null
                    };
                }

                // 获取管理员管理的场地
                var managedVenues = _context.VenueManagements
                    .Where(vm => vm.AdminId == adminId)
                    .Select(vm => new VenueDto
                    {
                        VenueId = vm.VenueId,
                        Name = _context.Venues.FirstOrDefault(v => v.VenueId == vm.VenueId).Name,
                        Type = _context.Venues.FirstOrDefault(v => v.VenueId == vm.VenueId).Type
                    })
                    .ToList();

                // 获取管理员管理的设备
                var managedEquipment = _context.Equipments
                    .Where(e => e.AdminId == adminId)
                    .Select(e => new EquipmentDto
                    {
                        EquipmentId = e.EquipmentId,
                        EquipmentName = e.EquipmentName
                    })
                    .ToList();

                // 返回结果
                return new AdminManagedItemsResultDto
                {
                    State = 1,  // 成功状态
                    Info = "获取成功",
                    Data = new AdminManagedItemsDto
                    {
                        ManagedVenues = managedVenues,
                        ManagedEquipment = managedEquipment
                    }
                };
            }
            catch (Exception ex)
            {
                return new AdminManagedItemsResultDto
                {
                    State = 0,
                    Info = $"获取数据失败: {ex.Message}",
                    Data = null
                };
            }
        }



        public RegisterResult RegisterAdmin(AdminDto adminDto, List<string> manageVenues,string? systemAdminId = null)
        {
            try
            {
                // 检查管理员是否已存在
                if (_adminRepository.Find(a => a.ContactNumber == adminDto.ContactNumber).Any())
                {
                    return new RegisterResult
                    {
                        State = 0,
                        AdminId = string.Empty,
                        Info = "管理员已存在"
                    };
                }

                // 生成唯一的管理员ID
                string adminId = GenerateUniqueAdminId();

                // 创建管理员实体
                var admin = new Admin
                {
                    AdminId = adminId,
                    RealName = adminDto.RealName,
                    Password = adminDto.Password,
                    ContactNumber = adminDto.ContactNumber,
                    AdminType = adminDto.AdminType
                };

                // 添加管理员到数据库
                _adminRepository.Add(admin);

                // 如果 manageVenues 不为空，则为每个场地分配管理员
                if (manageVenues != null && manageVenues.Any())
                {
                    foreach (var venueId in manageVenues)
                    {
                        var manageVenue = new VenueManagement
                        {
                            AdminId = adminId,
                            VenueId = venueId
                        };
                        _context.VenueManagements.Add(manageVenue);
                    }
                }

                // 检查系统管理员ID是否有效且为system类型，如果没有传递adminId，则使用默认系统管理员
                Admin? systemAdmin = null;
                if (!string.IsNullOrEmpty(systemAdminId))
                {
                    systemAdmin = _adminRepository.Find(a => a.AdminId == systemAdminId && a.AdminType == "system").FirstOrDefault();
                }
                if (systemAdmin == null)
                {
                    systemAdmin = _adminRepository.Find(a => a.AdminType == "system").FirstOrDefault();
                    if (systemAdmin == null)
                    {
                        return new RegisterResult
                        {
                            State = 0,
                            AdminId = string.Empty,
                            Info = "未找到系统管理员"
                        };
                    }
                }

                // 创建通知实体
                var notification = new AdminNotification
                {
                    AdminId = systemAdmin.AdminId,
                    NotificationId = GenerateUniqueNotificationId(),
                    NotificationType = "adminValidation",
                    Title = "管理员注册申请",
                    Content = $"管理员 [{admin.RealName}] 申请注册成为 [{admin.AdminType}]，联系电话为 {admin.ContactNumber}，申请说明为 [{adminDto.ApplyDescription}]，申请管理的场地为 {string.Join(", ", manageVenues)}",
                    NotificationTime = DateTime.UtcNow,
                    NewAdminId = adminId // 将新管理员ID存储到通知表中
                };

                _adminNotificationRepository.Add(notification);

                // 保存更改到数据库
                _context.SaveChanges();

                return new RegisterResult
                {
                    State = 1,
                    AdminId = adminId,
                    Info = "注册成功"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResult
                {
                    State = 0,
                    AdminId = string.Empty,
                    Info = $"注册失败: {ex.Message}"
                };
            }
        }


        // 生成唯一的管理员ID
        private string GenerateUniqueAdminId()
        {
            var random = new Random();
            string adminId;

            do
            {
                adminId = random.Next(10000, 99999).ToString();
            } while (_adminRepository.Find(a => a.AdminId == adminId).Any()); // 确保ID唯一性

            return adminId;
        }

        // 生成唯一的通知ID
        private string GenerateUniqueNotificationId()
        {
            // 获取当前数据库中最大的通知ID
            var maxNotificationId = _adminNotificationRepository.GetAll()
                .Select(n => Convert.ToInt32(n.NotificationId))
                .DefaultIfEmpty(0) // 如果没有记录，则返回0
                .Max();

            // 生成下一个通知ID
            int newNotificationId = maxNotificationId + 1;

            // 返回新的通知ID，转换为字符串
            return newNotificationId.ToString();
        }

        public UpdateResult UpdateAdminInfo(string adminId, AdminUpdateDto adminUpdateDto, List<string> manageVenues)
        {
            var admin = _adminRepository.Find(a => a.AdminId == adminId).FirstOrDefault();
            
            if (admin == null)
            {
                return new UpdateResult
                {
                    State = 0,
                    Info = "管理员未找到"
                };
            }

            // 更新管理员信息
            admin.RealName = adminUpdateDto.RealName;
            admin.ContactNumber = adminUpdateDto.ContactNumber;
            admin.AdminType = adminUpdateDto.AdminType;
            
            _adminRepository.Update(admin);

            // 管理场地的处理
            var currentManagedVenues = _context.VenueManagements.Where(vm => vm.AdminId == adminId).ToList();

            // 如果 manageVenues 为空，表示删除该管理员管理的所有场地
            if (manageVenues == null || !manageVenues.Any())
            {
                _context.VenueManagements.RemoveRange(currentManagedVenues);
            }
            else
            {
                // 添加新场地管理
                var newVenues = manageVenues.Except(currentManagedVenues.Select(vm => vm.VenueId)).ToList();
                foreach (var venueId in newVenues)
                {
                    var manageVenue = new VenueManagement
                    {
                        AdminId = adminId,
                        VenueId = venueId
                    };
                    _context.VenueManagements.Add(manageVenue);
                }

                // 删除不再管理的场地
                var removedVenues = currentManagedVenues.Where(vm => !manageVenues.Contains(vm.VenueId)).ToList();
                _context.VenueManagements.RemoveRange(removedVenues);
            }

            _context.SaveChanges();

            return new UpdateResult
            {
                State = 1,
                Info = "管理员信息更新成功"
            };
        }

        public UpdateResult UpdateAdminPassword(string adminId, string newPassword)
        {
            var admin = _adminRepository.Find(a => a.AdminId == adminId).FirstOrDefault();
            
            if (admin == null)
            {
                return new UpdateResult
                {
                    State = 0,
                    Info = "管理员未找到"
                };
            }

            // 更新管理员密码
            admin.Password = newPassword; // 假设传入的密码已经过加密

            _adminRepository.Update(admin);
            _context.SaveChanges();

            return new UpdateResult
            {
                State = 1,
                Info = "密码更新成功"
            };
        }







        // 其他方法的实现...
    }
}
