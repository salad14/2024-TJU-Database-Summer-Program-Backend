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
}

