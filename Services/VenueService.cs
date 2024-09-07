using System.Linq;
using VenueBookingSystem.Models;
using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using Microsoft.EntityFrameworkCore;

namespace VenueBookingSystem.Services
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _venueRepository;
        private readonly ApplicationDbContext _context;

        // 构造函数，注入存储库和数据库上下文
        public VenueService(IRepository<Venue> venueRepository, ApplicationDbContext context)
        {
            _venueRepository = venueRepository;
            _context = context;
        }

        // 获取所有场地信息
        public IEnumerable<VenueDto> GetAllVenueInfos()
        {
            return _context.Venues.Select(v => new VenueDto
            {
                VenueId = v.VenueId,
                Name = v.Name,
                Type = v.Type,
                Capacity = v.Capacity,
                Status = v.Status,
                MaintenanceCount = v.MaintenanceCount,
                LastInspectionTime = v.LastInspectionTime,
                VenueLocation = v.VenueLocation,
                VenueImageUrl = v.VenueImageUrl
            }).ToList();
        }

        //获取场地管理员和场地公告信息
        public VenueAdminAndAnnouncementResult GetVenueAdminAndAnnouncements(string venueId)
        {
            // 查找场地
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null)
            {
                return new VenueAdminAndAnnouncementResult
                {
                    State = 0,
                    Info = "未找到场地信息",
                    Data = null
                };
            }

            // 查找管理该场地的管理员信息
            var admin = _context.VenueManagements
                .Where(vm => vm.VenueId == venueId)
                .Select(vm => new AdminResponseDto
                {
                    AdminId = vm.Admin.AdminId,
                    RealName = vm.Admin.RealName,
                    ContactNumber = vm.Admin.ContactNumber,
                    AdminType = vm.Admin.AdminType
                })
                .ToList();

            // if (admin == null)
            // {
            //     return new VenueAdminAndAnnouncementResult
            //     {
            //         State = 0,
            //         Info = "未找到管理员信息",
            //         Data = null
            //     };
            // }

            // 使用新的 VenueAnnouncementResponseDto 查找公告信息
            var announcements = _context.VenueAnnouncements
                .Where(va => va.VenueId == venueId)
                .Select(va => new VenueAnnouncementResponseDto
                {
                    Title = va.Announcement.Title,
                    Content = va.Announcement.Content,
                    PublishDate = va.Announcement.PublishedDate
                })
                .ToList();

            return new VenueAdminAndAnnouncementResult
            {
                State = 1,
                Info = "",
                Data = new VenueAdminAndAnnouncementDto
                {
                    VenueId = venueId,
                    Admin = admin,
                    Announcements = announcements // 确保这里使用新的类型
                }
            };
        }



        // 添加新场地
        public AddVenueResult AddVenue(VenueDto venueDto)
        {

            // 检查场地名称是否已存在
            var venueExists = _context.Venues
                .Where(v => v.Name == venueDto.Name)
                .Select(v => 1)
                .FirstOrDefault() == 1;

            if (venueExists)
            {
                return new AddVenueResult
                {
                    VenueId = null,
                    Info = "场地名称已存在"
                };
            }
            // 获取当前最大的场地ID并递增
            var maxVenueId = _context.Venues
                .OrderByDescending(v => v.VenueId)
                .Select(v => v.VenueId)
                .FirstOrDefault();

            int newVenueId = (maxVenueId != null ? int.Parse(maxVenueId) : 100000) + 1;

            var venue = new Venue
            {
                VenueId = newVenueId.ToString(),
                Name = venueDto.Name,
                Type = venueDto.Type,
                Capacity = venueDto.Capacity,
                VenueDescription = venueDto.VenueDescription,
                VenueLocation = venueDto.VenueLocation,
                VenueImageUrl = venueDto.VenueImageUrl
            };

            try
            {
                _context.Venues.Add(venue);
                _context.SaveChanges();

                return new AddVenueResult
                {
                    VenueId = newVenueId.ToString(),
                    Info = ""
                };
            }
            catch (Exception ex)
            {
                return new AddVenueResult
                {
                    VenueId = null,
                    Info = $"添加场地时出错：{ex.Message}"
                };
            }
        }

        //编辑场地信息
        public EditVenueResult EditVenue(string venueId, VenueDto venueDto)
        {
            // 查找场地
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null)
            {
                return new EditVenueResult
                {
                    State = 0,
                    Info = "未找到该场地"
                };
            }

            // 更新场地信息
            venue.Name = venueDto.Name;
            venue.Type = venueDto.Type;
            venue.Status = venueDto.Status;
            venue.Capacity = venueDto.Capacity;
            venue.VenueImageUrl = venueDto.VenueImageUrl;
            venue.VenueLocation = venueDto.VenueLocation;
            venue.VenueDescription = venueDto.VenueDescription;

            try
            {
                // 保存修改
                _context.SaveChanges();
                return new EditVenueResult
                {
                    State = 1,
                    Info = "场地信息修改成功"
                };
            }
            catch (Exception ex)
            {
                return new EditVenueResult
                {
                    State = 0,
                    Info = $"修改场地信息时出错：{ex.Message}"
                };
            }
        }



        // 获取所有不同ID的场地
        public IEnumerable<VenueDto> GetAllVenueDetails()
        {
            return _context.Venues.Select(v => new VenueDto
            {
                VenueId = v.VenueId, // 确保场地ID被返回
                Name = v.Name,
                Type = v.Type,
                Capacity = v.Capacity,
                Status = v.Status
            }).ToList();
        }

        // 新增：获取所有维修记录及相关设备和场地信息
        public IEnumerable<RepairDataDto> GetAllRepairRecords()
        {
            var repairData = from record in _context.MaintenanceRecords
                             join equipment in _context.Equipments on record.EquipmentId equals equipment.EquipmentId
                             join venueEquipment in _context.VenueEquipments on equipment.EquipmentId equals venueEquipment.EquipmentId
                             join venue in _context.Venues on venueEquipment.VenueId equals venue.VenueId
                             select new RepairDataDto
                             {
                                 MaintenanceRecordId = record.MaintenanceRecordId,
                                 MaintenanceStartTime = record.MaintenanceStartTime,
                                 MaintenanceEndTime = record.MaintenanceEndTime,
                                 MaintenanceDetails = record.MaintenanceDetails,
                                 EquipmentId = equipment.EquipmentId,
                                 EquipmentName = equipment.EquipmentName,
                                 VenueId = venue.VenueId,
                                 VenueName = venue.Name
                             };

            return repairData.ToList(); // 将查询结果转换为列表返回
        }
        // 获取场地的详细信息
        public VenueDetailsDto GetVenueDetails(string venueId)
        {
            // 查找场地信息
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null) return null;

            // 查找设备信息
            var venueDevices = _context.Equipments
                .Join(_context.VenueEquipments, e => e.EquipmentId, ve => ve.EquipmentId, (e, ve) => new { e, ve })
                .Where(joined => joined.ve.VenueId == venueId)
                .Select(joined => new EquipmentDto
                {
                    EquipmentId = joined.e.EquipmentId,
                    EquipmentName = joined.e.EquipmentName
                }).ToList();

            // 查找保养记录
            var maintenanceRecords = _context.VenueMaintenances
                .Where(vm => vm.VenueId == venueId)
                .Select(vm => new VenueMaintenanceDto
                {
                    VenueMaintenanceId = vm.VenueMaintenanceId,
                    MaintenanceStartDate = vm.MaintenanceStartDate,
                    MaintenanceEndDate = vm.MaintenanceEndDate,
                    Description = vm.Description
                }).ToList();

            // 返回场地详细信息
            return new VenueDetailsDto
            {
                VenueId = venue.VenueId,
                VenueDescription = venue.VenueDescription,
                Name = venue.Name,
                Type = venue.Type,
                Capacity = venue.Capacity,
                Status = venue.Status,
                VenueDevices = venueDevices,
                MaintenanceRecords = maintenanceRecords,
                VenueLocation = venue.VenueLocation,  // 假设场地位置在 venue.Location 中
                VenueImageUrl = venue.VenueImageUrl   // 假设场地图像链接在 venue.ImageUrl 中
            };
        }
        // 获取设备的详细信息
        public EquipmentDetailsDto GetEquipmentDetails(string equipmentId)
        {
            var equipment = _context.Equipments.FirstOrDefault(e => e.EquipmentId == equipmentId);
            if (equipment == null) return null;

            var venueEquipment = _context.VenueEquipments.FirstOrDefault(ve => ve.EquipmentId == equipmentId);
            var venue = venueEquipment != null ? _context.Venues.FirstOrDefault(v => v.VenueId == venueEquipment.VenueId) : null;

            var repairRecords = _context.MaintenanceRecords
                .Where(mr => mr.EquipmentId == equipmentId)
                .Select(mr => new RepairDataDto
                {
                    MaintenanceRecordId = mr.MaintenanceRecordId,
                    MaintenanceStartTime = mr.MaintenanceStartTime,
                    MaintenanceEndTime = mr.MaintenanceEndTime,
                    MaintenanceDetails = mr.MaintenanceDetails
                }).ToList();

            return new EquipmentDetailsDto
            {
                EquipmentId = equipment.EquipmentId,
                EquipmentName = equipment.EquipmentName,
                EquipmentStatus = venueEquipment != null ? "Active" : "Inactive", // 假设状态根据是否有场地来定义
                EquipmentIntroTime = venueEquipment != null ? venueEquipment.InstallationTime : DateTime.MinValue,
                VenueId = venue?.VenueId,
                VenueName = venue?.Name,
                RepairRecords = repairRecords
            };
        }

        // 添加设备信息
        public AddDeviceResult AddDevice(string adminId, string equipmentName, string venueId, DateTime? installationTime)
        {
            // 查找当前表中最大的设备ID
            var maxEquipmentId = _context.VenueEquipments
                .OrderByDescending(ve => ve.EquipmentId)
                .Select(ve => ve.EquipmentId)
                .FirstOrDefault();

            // 如果没有设备记录，则从 1 开始，否则加 1
            int newEquipmentId = 1;
            if (!string.IsNullOrEmpty(maxEquipmentId))
            {
                if (int.TryParse(maxEquipmentId, out int maxId))
                {
                    newEquipmentId = maxId + 1;
                }
            }

            // 添加设备到设备表
            var equipment = new Equipment
            {
                EquipmentId = newEquipmentId.ToString(), // 将整数 ID 转换为字符串
                EquipmentName = equipmentName,
                AdminId = adminId
            };

            _context.Equipments.Add(equipment);

            // 如果场地ID不为空，则添加设备和场地的关系
            if (!string.IsNullOrEmpty(venueId))
            {
                var venueEquipment = new VenueEquipment
                {
                    VenueId = venueId,
                    EquipmentId = newEquipmentId.ToString(), // 使用新的设备ID
                    InstallationTime = installationTime ?? DateTime.Now
                };
                _context.VenueEquipments.Add(venueEquipment);
            }

            // 保存更改并返回结果
            try
            {
                _context.SaveChanges();
                return new AddDeviceResult
                {
                    State = 1,
                    DeviceId = newEquipmentId.ToString(),
                    Info = "设备添加成功"
                };
            }
            catch (Exception ex)
            {
                return new AddDeviceResult
                {
                    State = 0,
                    DeviceId = "",
                    Info = $"设备添加失败: {ex.Message}"
                };
            }
        }

        // 编辑设备信息
        public EditDeviceResult EditDevice(string equipmentId, string equipmentName, string venueId)
        {
            // 查找设备
            var equipment = _context.Equipments.FirstOrDefault(e => e.EquipmentId == equipmentId);
            if (equipment == null)
            {
                return new EditDeviceResult
                {
                    State = 0,
                    Info = "设备未找到"
                };
            }

            // 更新设备名称
            equipment.EquipmentName = equipmentName;

            // 如果场地ID不为空，则更新场地设备关系表
            if (!string.IsNullOrEmpty(venueId))
            {
                var venueEquipment = _context.VenueEquipments.FirstOrDefault(ve => ve.EquipmentId == equipmentId);
                if (venueEquipment != null)
                {
                    // 更新场地ID和设备引进时间
                    venueEquipment.VenueId = venueId;
                    venueEquipment.InstallationTime = DateTime.Now; // 设置为当前时间
                }
                else
                {
                    // 如果没有场地设备关系，创建新的关联
                    venueEquipment = new VenueEquipment
                    {
                        EquipmentId = equipmentId,
                        VenueId = venueId,
                        InstallationTime = DateTime.Now
                    };
                    _context.VenueEquipments.Add(venueEquipment);
                }
            }

            // 保存修改并返回结果
            try
            {
                _context.SaveChanges();
                return new EditDeviceResult
                {
                    State = 1,
                    Info = "设备信息更新成功"
                };
            }
            catch (Exception ex)
            {
                return new EditDeviceResult
                {
                    State = 0,
                    Info = $"修改设备信息时出错：{ex.Message}"
                };
            }
        }
        // 添加维修信息
        public AddRepairResult AddRepair(string equipmentId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails)
        {
            // 查找当前表中最大的维修记录ID
            var maxRepairId = _context.MaintenanceRecords
                .OrderByDescending(mr => mr.MaintenanceRecordId)
                .Select(mr => mr.MaintenanceRecordId)
                .FirstOrDefault();

            // 如果没有维修记录，则从 1 开始，否则加 1
            int newRepairId = 1;
            if (!string.IsNullOrEmpty(maxRepairId))
            {
                if (int.TryParse(maxRepairId, out int maxId))
                {
                    newRepairId = maxId + 1;
                }
            }

            // 检查设备是否存在
            var equipment = _context.Equipments.FirstOrDefault(e => e.EquipmentId == equipmentId);
            if (equipment == null)
            {
                return new AddRepairResult
                {
                    State = 0,
                    RepairId = "",
                    Info = "设备未找到"
                };
            }

            // 添加维修记录
            var maintenanceRecord = new MaintenanceRecord
            {
                MaintenanceRecordId = newRepairId.ToString(), // 将整数 ID 转换为字符串
                EquipmentId = equipmentId,
                MaintenanceStartTime = maintenanceStartTime,
                MaintenanceEndTime = maintenanceEndTime,
                MaintenanceDetails = maintenanceDetails
            };

            _context.MaintenanceRecords.Add(maintenanceRecord);

            // 保存更改并返回结果
            try
            {
                _context.SaveChanges();
                return new AddRepairResult
                {
                    State = 1,
                    RepairId = newRepairId.ToString(),
                    Info = "维修记录添加成功"
                };
            }
            catch (Exception ex)
            {
                return new AddRepairResult
                {
                    State = 0,
                    RepairId = "",
                    Info = $"添加维修记录时出错：{ex.Message}"
                };
            }
        }

        // 编辑维修信息
        public EditRepairResult EditRepair(string repairId, DateTime maintenanceStartTime, DateTime maintenanceEndTime, string maintenanceDetails)
        {
            // 查找维修记录
            var maintenanceRecord = _context.MaintenanceRecords.FirstOrDefault(m => m.MaintenanceRecordId == repairId);
            if (maintenanceRecord == null)
            {
                return new EditRepairResult
                {
                    State = 0,
                    Info = "未找到维修记录"
                };
            }

            // 更新维修记录的字段
            maintenanceRecord.MaintenanceStartTime = maintenanceStartTime;
            maintenanceRecord.MaintenanceEndTime = maintenanceEndTime;
            maintenanceRecord.MaintenanceDetails = maintenanceDetails;

            // 保存修改并返回结果
            try
            {
                _context.SaveChanges();
                return new EditRepairResult
                {
                    State = 1,
                    Info = "维修记录更新成功"
                };
            }
            catch (Exception ex)
            {
                return new EditRepairResult
                {
                    State = 0,
                    Info = $"更新维修记录时出错：{ex.Message}"
                };
            }
        }
        // 根据日期查询场地的开放时间段
        public IEnumerable<AvailabilityDto> GetVenueAvailabilityByDate(string venueId, DateTime date)
        {
            var availabilities = _context.VenueAvailabilities
                .Where(va => va.VenueId == venueId && va.StartTime.Date == date.Date)
                .Select(va => new AvailabilityDto
                {
                    AvailabilityId = va.AvailabilityId,
                    StartTime = va.StartTime,
                    EndTime = va.EndTime,
                    Price = va.Price,
                    RemainingCapacity = va.RemainingCapacity
                }).ToList();

            return availabilities;
        }
        // 添加保养信息
        public AddMaintenanceResult AddMaintenance(string venueId, DateTime maintenanceStartDate, DateTime maintenanceEndDate, string description)
        {
            // 查找当前表中的最大VenueMaintenanceId
            var maxIdString = _context.VenueMaintenances
                .OrderByDescending(vm => vm.VenueMaintenanceId)
                .Select(vm => vm.VenueMaintenanceId)
                .FirstOrDefault();

            // 如果表中没有记录，从100001开始；否则将最大值加1
            long newMaintenanceId;
            if (string.IsNullOrEmpty(maxIdString))
            {
                newMaintenanceId = 1; // 初始值
            }
            else
            {
                // 将字符串转换为数字并加1
                newMaintenanceId = long.Parse(maxIdString) + 1;
            }

            // 添加保养记录
            var venueMaintenance = new VenueMaintenance
            {
                VenueMaintenanceId = newMaintenanceId.ToString(), // 使用递增的ID，保存为字符串
                VenueId = venueId,
                MaintenanceStartDate = maintenanceStartDate,
                MaintenanceEndDate = maintenanceEndDate,
                Description = description
            };

            _context.VenueMaintenances.Add(venueMaintenance);

            // 保存更改并返回结果
            try
            {
                _context.SaveChanges();
                return new AddMaintenanceResult
                {
                    State = 1,
                    MaintenanceId = newMaintenanceId.ToString(), // 返回新生成的ID
                    Info = "保养记录添加成功"
                };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += $" | 内部错误: {ex.InnerException.Message}";
                }

                return new AddMaintenanceResult
                {
                    State = 0,
                    MaintenanceId = "",
                    Info = $"添加保养记录时出错：{errorMessage}"
                };
            }
        }



        // 修改保养信息
        public EditMaintenanceResult EditMaintenance(string maintenanceId, DateTime maintenanceStartDate, DateTime maintenanceEndDate, string description)
        {
            // 查找保养记录
            var venueMaintenance = _context.VenueMaintenances.FirstOrDefault(vm => vm.VenueMaintenanceId == maintenanceId);
            if (venueMaintenance == null)
            {
                // 如果没有找到保养记录，返回错误信息
                return new EditMaintenanceResult
                {
                    State = 0,
                    Info = "未找到保养记录"
                };
            }

            // 更新保养记录的字段
            venueMaintenance.MaintenanceStartDate = maintenanceStartDate;
            venueMaintenance.MaintenanceEndDate = maintenanceEndDate;
            venueMaintenance.Description = description;

            // 尝试保存更改并返回结果
            try
            {
                _context.SaveChanges();
                return new EditMaintenanceResult
                {
                    State = 1,
                    Info = "保养记录更新成功"
                };
            }
            catch (Exception ex)
            {
                // 捕获数据库异常，并返回详细错误信息
                return new EditMaintenanceResult
                {
                    State = 0,
                    Info = $"更新保养记录时出错：{ex.Message}"
                };
            }
        }

        // 修改开放时间段信息
        public EditAvailabilityResult EditAvailability(string availabilityId, DateTime startTime, DateTime endTime, decimal price, int remainingCapacity)
        {
            // 查找开放时间段记录
            var availability = _context.VenueAvailabilities.FirstOrDefault(va => va.AvailabilityId == availabilityId);
            if (availability == null)
            {
                return new EditAvailabilityResult
                {
                    State = 0,
                    Info = "未找到开放时间段记录"
                };
            }

            // 更新开放时间段记录的字段
            availability.StartTime = startTime;
            availability.EndTime = endTime;
            availability.Price = price;
            availability.RemainingCapacity = remainingCapacity;

            // 保存修改并返回结果
            try
            {
                _context.SaveChanges();
                return new EditAvailabilityResult
                {
                    State = 1,
                    Info = "开放时间段记录更新成功"
                };
            }
            catch (Exception ex)
            {
                return new EditAvailabilityResult
                {
                    State = 0,
                    Info = $"更新开放时间段记录时出错：{ex.Message}"
                };
            }
        }
        // 添加开放时间段
        public AddAvailabilityResult AddAvailability(string venueId, DateTime startTime, DateTime endTime, decimal price, int remainingCapacity)
        {
            // 使用 AsNoTracking() 查询，避免跟踪问题
            var venue = _context.Venues.AsNoTracking().FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null)
            {
                return new AddAvailabilityResult
                {
                    State = 0,
                    AvailabilityId = "",
                    Info = "场地未找到"
                };
            }

            // 查找当前表中所有的开放时间段ID并拉回内存
            var availabilityIds = _context.VenueAvailabilities
                .Select(va => va.AvailabilityId)
                .ToList()  // 将查询结果拉回内存
                .Where(id => int.TryParse(id, out _))  // 过滤出可以转换为整数的ID
                .OrderByDescending(id => Convert.ToInt32(id))  // 转换为整数并排序
                .FirstOrDefault();  // 获取最大的ID

            Console.WriteLine($"最大 AvailabilityId: {availabilityIds}");  // 输出实际的最大ID

            // 如果数据库中没有任何记录，则从 "1" 开始
            int newAvailabilityId = 1;
            if (!string.IsNullOrEmpty(availabilityIds) && int.TryParse(availabilityIds, out int maxId))
            {
                newAvailabilityId = maxId + 1;
            }

            Console.WriteLine($"生成的新 AvailabilityId: {newAvailabilityId}");  // 输出生成的新ID

            // 将生成的 ID 转换为字符串
            string availabilityId = newAvailabilityId.ToString();

            // 创建并保存新的 VenueAvailability 记录
            var venueAvailability = new VenueAvailability
            {
                AvailabilityId = availabilityId,
                VenueId = venueId,
                StartTime = startTime,
                EndTime = endTime,
                Price = price,
                RemainingCapacity = remainingCapacity
            };

            // 添加新的 VenueAvailability 实体
            _context.VenueAvailabilities.Add(venueAvailability);

            try
            {
                // 保存更改
                _context.SaveChanges();
                return new AddAvailabilityResult
                {
                    State = 1,
                    AvailabilityId = availabilityId,
                    Info = "开放时间段添加成功"
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return new AddAvailabilityResult
                {
                    State = 0,
                    AvailabilityId = "",
                    Info = $"添加开放时间段时出错：{innerException}"
                };
            }
            catch (Exception ex)
            {
                return new AddAvailabilityResult
                {
                    State = 0,
                    AvailabilityId = "",
                    Info = $"发生意外错误：{ex.Message}"
                };
            }
        }


        // 删除开放时间段
        public DeleteAvailabilityResult DeleteAvailability(string availabilityId)
        {
            // 查找开放时间段记录
            var availability = _context.VenueAvailabilities.FirstOrDefault(va => va.AvailabilityId == availabilityId);
            if (availability == null)
            {
                return new DeleteAvailabilityResult
                {
                    State = 0,
                    Info = "未找到开放时间段记录"
                };
            }

            // 删除记录
            _context.VenueAvailabilities.Remove(availability);

            // 保存修改并返回结果
            try
            {
                _context.SaveChanges();
                return new DeleteAvailabilityResult
                {
                    State = 1,
                    Info = "开放时间段记录删除成功"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAvailabilityResult
                {
                    State = 0,
                    Info = $"删除开放时间段记录时出错：{ex.Message}"
                };
            }
        }

        public VenueAnnouncementDto GetVenueDetailsAnnouncement(string venueId)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null) return null;

            var vAdmin = _context.VenueManagements
                .Where(va => va.VenueId == venueId)
                .Select(va => new VenueAdminDto
                {
                    AdminId = va.AdminId,
                    RealName = va.Admin.RealName,
                }).ToList();

            List<VenueAnnouncementVDto> vaList = new List<VenueAnnouncementVDto>();
            if (venue.VenueAnnouncements != null && venue.VenueAnnouncements.Count() > 0)
            {
                foreach (var item in venue.VenueAnnouncements)
                {
                    var announcements = _context.Announcements
                        .Where(va => va.AnnouncementId == item.AnnouncementId)
                        .Select(va => new VenueAnnouncementVDto
                        {
                            AnnouncementId = va.AnnouncementId,
                            Title = va.Title,
                            PublishedDate = va.PublishedDate,
                            LastModifiedDate = va.LastModifiedDate
                        }).FirstOrDefault();
                    if (announcements != null)
                    {
                        vaList.Add(announcements);
                    }
                }
            }

            return new VenueAnnouncementDto
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                Type = venue.Type,
                VenueDescription = venue.VenueDescription,
                VenueAdminDto = vAdmin,
                VenueAnnouncementsDto = vaList
            };
        }

        public List<EquipmentDetailsDto> GetAllEquipment()
        {
            var equipments = _context.Equipments.AsEnumerable();
            var equipmentDetails = new List<EquipmentDetailsDto>();
            foreach (var equipment in equipments)
            {
                var venueEquipment = _context.VenueEquipments.FirstOrDefault(ve => ve.EquipmentId == equipment.EquipmentId);
                var venue = venueEquipment != null ? _context.Venues.FirstOrDefault(v => v.VenueId == venueEquipment.VenueId) : null;
                var repairRecords = _context.MaintenanceRecords
                    .Where(mr => mr.EquipmentId == equipment.EquipmentId)
                    .Select(mr => new RepairDataDto
                    {
                        MaintenanceRecordId = mr.MaintenanceRecordId,
                        MaintenanceStartTime = mr.MaintenanceStartTime,
                        MaintenanceEndTime = mr.MaintenanceEndTime,
                        MaintenanceDetails = mr.MaintenanceDetails
                    }).ToList();

                equipmentDetails.Add(new EquipmentDetailsDto
                {
                    EquipmentId = equipment.EquipmentId,
                    EquipmentName = equipment.EquipmentName,
                    EquipmentStatus = venueEquipment != null ? "Active" : "Inactive", // 假设状态根据是否有场地来定义
                    EquipmentIntroTime = venueEquipment != null ? venueEquipment.InstallationTime : DateTime.MinValue,
                    VenueId = venue?.VenueId,
                    VenueName = venue?.Name,
                    RepairRecords = repairRecords,
                });
            }
            return equipmentDetails;
        }
        public UpdateVenueAdminResult UpdateVenueAdmin(string venueId, IEnumerable<string> adminArr)
        {
            var existingAdmins = _context.VenueManagements.Where(x => x.VenueId == venueId)
            .Select(y => y.AdminId).AsEnumerable();
            var newAdmins = adminArr.Except(existingAdmins);
            foreach (var admin in newAdmins)
            {
                // if (!_context.Admins.Where(x => x.AdminId == admin).Any())
                // {
                //     return new UpdateVenueAdminResult
                //     {
                //         State = 0,
                //         Info = "未找到管理员",
                //     };
                // }
                var venueAdmin = new VenueManagement
                {
                    VenueId = venueId,
                    AdminId = admin
                };
                _context.VenueManagements.Add(venueAdmin);
            }
            _context.SaveChanges();
            var kickedAdmins = existingAdmins.Except(adminArr);
            foreach (var admin in newAdmins)
            {
                var venueAdmin = new VenueManagement
                {
                    VenueId = venueId,
                    AdminId = admin
                };
                _context.VenueManagements.Remove(venueAdmin);
            };
            _context.SaveChanges();
            return new UpdateVenueAdminResult
            {
                State = 1,
                Info = ""
            };
        }

        // 其他场地服务逻辑...
    }
}