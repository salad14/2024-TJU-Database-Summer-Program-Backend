// RepairDataDto.cs
namespace VenueBookingSystem.Models
{
    public class RepairDataDto
    {
        public string MaintenanceRecordId { get; set; }  // 维修记录ID
        public DateTime MaintenanceStartTime { get; set; }  // 维修开始时间
        public DateTime MaintenanceEndTime { get; set; }  // 维修结束时间
        public string MaintenanceDetails { get; set; }  // 维修描述
        public string EquipmentId { get; set; }  // 设备ID
        public string EquipmentName { get; set; }  // 设备名称
        public string VenueId { get; set; }  // 场地ID
        public string VenueName { get; set; }  // 场地名称
    }

    public class AddDeviceRequest
    {
        public string AdminId { get; set; } // 管理员ID
        public string EquipmentName { get; set; } // 设备名称
        public string VenueId { get; set; } // 场地ID（可为空）
        public DateTime? InstallationTime { get; set; } // 设备引进时间（可为空）
    } 

    public class EditDeviceRequest
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
        public string VenueId { get; set; } // 场地ID
    }
    
    public class AddRepairRequest
    {
        public string EquipmentId { get; set; } // 设备ID
        public DateTime MaintenanceStartTime { get; set; } // 维修开始时间
        public DateTime MaintenanceEndTime { get; set; } // 维修结束时间
        public string MaintenanceDetails { get; set; } // 维修描述
    }
    public class EditRepairRequest
    {
        public string RepairId { get; set; } // 维修记录ID
        public DateTime MaintenanceStartTime { get; set; } // 维修开始时间
        public DateTime MaintenanceEndTime { get; set; } // 维修结束时间
        public string MaintenanceDetails { get; set; } // 维修描述
    }
    public class AddMaintenanceRequest
    {
        public string VenueId { get; set; } // 场地ID
        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间
        public string Description { get; set; } // 保养描述
    }
    public class EditMaintenanceRequest
    {
        public string MaintenanceId { get; set; } // 保养记录ID
        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间
        public string Description { get; set; } // 保养描述
    }
     public class EditAvailabilityRequest
    {
        public string AvailabilityId { get; set; } // 开放时间段ID
        public DateTime StartTime { get; set; } // 开放开始时间
        public DateTime EndTime { get; set; } // 开放结束时间
        public decimal Price { get; set; } // 场地价格
        public int RemainingCapacity { get; set; } // 剩余容量
    }
    public class AddAvailabilityRequest
    {
        public string VenueId { get; set; } // 场地ID
        public DateTime StartTime { get; set; } // 开放开始时间
        public DateTime EndTime { get; set; } // 开放结束时间
        public decimal Price { get; set; } // 场地价格
        public int RemainingCapacity { get; set; } // 剩余容量
    }

}