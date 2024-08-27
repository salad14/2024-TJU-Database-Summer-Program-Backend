namespace VenueBookingSystem.Models
{
    public class VenueMaintenance
    {
        public int VenueMaintenanceId { get; set; } // 唯一标识符
        public DateTime MaintenanceTime { get; set; } // 保养时间
        public string Description { get; set; } // 保养描述

        // 导航属性：关联的场地
        public string VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
