using VenueBookingSystem.Models;

namespace sports_management.Dto
{
    public class VenueMaintenanceDto
    {
<<<<<<< HEAD
        public required string VenueMaintenanceId { get; set; } // 唯一标识符
=======
        public string VenueMaintenanceId { get; set; } // 唯一标识符
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        public string Description { get; set; } // 保养描述

        public DateTime MaintenanceStartDate { get; set; } // 保养开始时间
        public DateTime MaintenanceEndDate { get; set; } // 保养结束时间

        // 导航属性：关联的场地
        public string VenueId { get; set; }

    }
}
