namespace VenueBookingSystem.Models
{
    public class VenueDetailsDto
    {
        public string VenueId { get; set; } // 场地ID
        public string Name { get; set; } // 场地名称
        public string Type { get; set; } // 运动类型
        public int Capacity { get; set; } // 可容纳人数
        public string Status { get; set; } // 场地状态
        public IEnumerable<VenueAvailabilityDto> OpenTime { get; set; } // 开放时间信息
        public IEnumerable<EquipmentDto> VenueDevices { get; set; } // 设备信息
        public IEnumerable<VenueMaintenanceDto> MaintenanceRecords { get; set; } // 保养记录
    }

    public class VenueAvailabilityDto
    {
        public string StartTime { get; set; } // 开始时间
        public string EndTime { get; set; } // 结束时间
        public int RemainingCapacity { get; set; } // 剩余容量
        public decimal Price { get; set; } // 场地价格
    }

    public class EquipmentDto
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
    }
}