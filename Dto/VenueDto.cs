namespace VenueBookingSystem.Models
{
    // 定义 VenueDto 类
    public class VenueDto
    {
        public required string Name { get; set; }  // 场地名称
        public required string Type { get; set; }  // 场地类型
        public int Capacity { get; set; }  // 场地容纳人数
        public string Status { get; set; } // 场地状态
        public int MaintenanceCount { get; set; } // 场地维护次数
        public DateTime LastInspectionTime { get; set; } // 场地上一次检查时间
        public string? VenueImageUrl { get; set; } // 场地图片 URL
<<<<<<< HEAD
        public string? VenueDescription{ get; set; } 
    }


    public class VADto
    {
        public  string VenueId { get; set; }  // 场地名称
        public  string Name { get; set; }  // 场地名称
        public  string Type { get; set; }  // 场地类型
        public int Capacity { get; set; }  // 场地容纳人数

=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
    }
}
