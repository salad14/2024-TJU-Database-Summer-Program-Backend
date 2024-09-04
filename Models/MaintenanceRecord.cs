namespace VenueBookingSystem.Models
{
    public class MaintenanceRecord
    {
        public string MaintenanceRecordId { get; set; } // 维护记录ID，主键
        public required string EquipmentId { get; set; } // 设备ID，外键，关联到Equipments表

        public DateTime MaintenanceStartTime { get; set; } // 维护开始时间
        public DateTime MaintenanceEndTime { get; set; } // 维护结束时间
        public string MaintenanceDetails { get; set; } // 维护描述

        // 导航属性：关联的设备
        public Equipment? Equipment { get; set; }
    }
}
