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

        public string VenueLocation { get; set; }
        public string? VenueDescription { get; set; }
    }

    public class VenueInfoDto
    {
        public string VenueId { get; set; }
        public string VenueName { get; set; }
    }

    public class VADto
    {
        public string VenueId { get; set; }  // 场地名称
        public string Name { get; set; }  // 场地名称
        public string Type { get; set; }  // 场地类型
        public int Capacity { get; set; }  // 场地容纳人数

    }

    public class VenueAdminAndAnnouncementDto
    {
        public string VenueId { get; set; }
        public List<AdminResponseDto> Admin { get; set; }
        public List<VenueAnnouncementResponseDto> Announcements { get; set; } // 修改为新类型
    }

    public class DeleteAvailabilityRequest
    {
        public string AvailabilityId { get; set; }  // 开放时间段ID
    }

    public class UpdateVenueAdminRequest
    {
        public string VenueId { get; set; }
        public IEnumerable<string> VenueAdmins { get; set; }
    }

}
