using System.Linq;
using VenueBookingSystem.Models;
using VenueBookingSystem.Data;

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

        // 获取所有场地
        public IEnumerable<Venue> GetAllVenues()
        {
            return _venueRepository.GetAll();
        }
        // 获取所有场地信息
        public IEnumerable<VenueDto> GetAllVenueInfos()
        {
            return _context.Venues.Select(v => new VenueDto
            {
                Name = v.Name,
                Type = v.Type,
                Capacity = v.Capacity,
                Status = v.Status,
                MaintenanceCount = v.MaintenanceCount,
                LastInspectionTime = v.LastInspectionTime,
                VenueImageUrl = v.VenueImageUrl
            }).ToList();
        }

        // 添加新场地
        public void AddVenue(VenueDto venueDto)
        {
            var venue = new Venue
            {
                Name = venueDto.Name,
                Type = venueDto.Type,
                Capacity = venueDto.Capacity
            };
            _venueRepository.Add(venue);
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
         // 获取指定场地的详细信息
        public VenueDetailsDto GetVenueDetails(string venueId)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == venueId);
            if (venue == null) return null;

            var openTime = _context.VenueAvailabilities
                .Where(va => va.VenueId == venueId)
                .Select(va => new VenueAvailabilityDto
                {
                    StartTime = va.StartTime.ToString("HH:mm"),
                    EndTime = va.EndTime.ToString("HH:mm"),
                    RemainingCapacity = va.RemainingCapacity,
                    Price = va.Price
                }).ToList();

            var venueDevices = _context.Equipments
                .Join(_context.VenueEquipments, e => e.EquipmentId, ve => ve.EquipmentId, (e, ve) => new { e, ve })
                .Where(joined => joined.ve.VenueId == venueId)
                .Select(joined => new EquipmentDto
                {
                    EquipmentId = joined.e.EquipmentId,
                    EquipmentName = joined.e.EquipmentName
                }).ToList();

            return new VenueDetailsDto
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                Type = venue.Type,
                Capacity = venue.Capacity,
                Status = venue.Status,
                OpenTime = openTime,
                VenueDevices = venueDevices
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
            // 生成唯一的设备ID
            string newEquipmentId = Guid.NewGuid().ToString();

            // 添加设备到设备表
            var equipment = new Equipment
            {
                EquipmentId = newEquipmentId,
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
                    EquipmentId = newEquipmentId,
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
                    DeviceId = newEquipmentId,
                    Info = ""
                };
            }
            catch (Exception ex)
            {
                return new AddDeviceResult
                {
                    State = 0,
                    DeviceId = "",
                    Info = $"Failed to add device: {ex.Message}"
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
            // 生成唯一的维修记录ID
            string newRepairId = Guid.NewGuid().ToString();

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
                MaintenanceRecordId = newRepairId,
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
                    RepairId = newRepairId,
                    Info = ""
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

        // 其他场地服务逻辑...
    }
}
