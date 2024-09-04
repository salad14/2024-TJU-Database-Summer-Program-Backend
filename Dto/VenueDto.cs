namespace VenueBookingSystem.Models
{
    // 定义 VenueDto 类
    public class VenueDto
    {
        public string? VenueId { get; set; }  // 场地名称
        public required string Name { get; set; }  // 场地名称
        public required string Type { get; set; }  // 场地类型
        public int Capacity { get; set; }  // 场地容纳人数
        public string Status { get; set; } // 场地状态
        public int MaintenanceCount { get; set; } // 场地维护次数
        public DateTime LastInspectionTime { get; set; } // 场地上一次检查时间
        public string? VenueImageUrl { get; set; } // 场地图片 URL
        public string? VenueDescription{ get; set; } 
    }

    public class VenueInfoDto
    {
        public string VenueId { get; set; }
        public string VenueName { get; set; }
    }

    
}
