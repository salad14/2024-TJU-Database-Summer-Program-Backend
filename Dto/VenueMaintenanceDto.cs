using VenueBookingSystem.Models;

namespace sports_management.Dto
{
    public class VenueMaintenanceDto
    {
        public required string VenueMaintenanceId { get; set; } // 唯一标识符
        public string Description { get; set; } // 保养描述

        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间

        // 导航属性：关联的场地
        public string VenueId { get; set; }

    }
}
