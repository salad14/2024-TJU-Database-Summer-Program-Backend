namespace VenueBookingSystem.Models
{
    public class Equipment
    {
        public required string EquipmentId { get; set; } // 设备ID，每件设备的唯一标识符
        
        public string? EquipmentName { get; set; } // 设备名称或描述

        // 导航属性：关联的管理员
        public required string AdminId { get; set; } // 管理员ID，设备管理人员ID
        public Admin Admin { get; set; }
    }
}
