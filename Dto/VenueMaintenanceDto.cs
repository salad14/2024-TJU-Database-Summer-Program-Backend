namespace VenueBookingSystem.Models
{
    public class VenueMaintenanceDto
    {
        public string VenueMaintenanceId { get; set; } // 保养ID
        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间
        public string Description { get; set; } // 保养描

        // 导航属性：关联的场地
        public string VenueId { get; set; }

    }
}
