using System.Collections.Generic;

namespace VenueBookingSystem.Models
{
    public class Venue
    {
        public string VenueId { get; set; } // 场地ID，类型更改为string以匹配数据库
        public required string Name { get; set; } // 场地名称
        public required string Type { get; set; } // 场地类型
        public int Capacity { get; set; } // 容纳人数
        public string Status { get; set; } = "y"; // 场地状态
        public int MaintenanceCount { get; set; } // 场地维护次数
        public DateTime LastInspectionTime { get; set; } // 场地上一次检查时间
        public string? VenueImageUrl { get; set; } // 场地图片 URL
        public string? VenueDescription{ get; set; } 

        public string? VenueLocation { get; set; } //场地位置


        // 导航属性：场地的预约记录
        public ICollection<Reservation>? Reservations { get; set; }
        // 导航属性：场地的保养记录
        public ICollection<VenueMaintenance> VenueMaintenances { get; set; }
        // 导航属性：场地的公告记录
        public ICollection<VenueAnnouncement> VenueAnnouncements { get; set; }

        // 导航属性：场地的开放时间段 (一对多)
        public ICollection<VenueAvailability>? VenueAvailabilities { get; set; } // 可空，ORM 将填充
    }
}
