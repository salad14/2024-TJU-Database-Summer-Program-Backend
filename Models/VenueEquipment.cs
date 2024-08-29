namespace VenueBookingSystem.Models
{
    public class VenueEquipment
    {
        public required string EquipmentId { get; set; } // 设备ID，每件设备的唯一标识符
        public required string VenueId { get; set; } // 场地ID，关联的场地唯一标识符
        public DateTime InstallationTime { get; set; } // 设备引进时间，设备被引进或安装在场地的时间

        // 导航属性：关联的设备和场地
        public Equipment Equipment { get; set; } // 关联的设备
        public Venue Venue { get; set; } // 关联的场地
    }
}
