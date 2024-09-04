namespace VenueBookingSystem.Models
{
    public class EquipmentDetailsDto
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
        public string EquipmentStatus { get; set; } // 设备状态
        public DateTime EquipmentIntroTime { get; set; } // 设备引入时间
        public string VenueId { get; set; } // 设备所在场地的ID
        public string VenueName { get; set; } // 设备所在场地的名称
        public IEnumerable<RepairDataDto> RepairRecords { get; set; } // 设备维修记录
    }
}
