namespace VenueBookingSystem.Models
{
    public class VenueDetailsDto
    {
        public string VenueId { get; set; } // 场地ID
        public string Name { get; set; } // 场地名称
        public string Type { get; set; } // 运动类型
        public int Capacity { get; set; } // 可容纳人数
        public string Status { get; set; } // 场地状态
        public IEnumerable<EquipmentDto> VenueDevices { get; set; } // 设备信息
        public IEnumerable<VenueMaintenanceDto> MaintenanceRecords { get; set; } // 保养记录
    }

    public class EquipmentDto
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
    }

    public class VenueMaintenanceDto
    {
        public string VenueMaintenanceId { get; set; } // 保养ID
        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间
        public string Description { get; set; } // 保养描述
    }
}
