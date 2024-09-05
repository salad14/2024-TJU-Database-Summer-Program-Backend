namespace VenueBookingSystem.Models
{
    public class VenueDetailsDto
    {
        public string VenueId { get; set; } // 场地ID
        public string Name { get; set; } // 场地名称
        public string Type { get; set; } // 运动类型
        public int Capacity { get; set; } // 可容纳人数
        public string Status { get; set; } // 场地状态

        public string VenueLocation { get; set; }  
        public string VenueImageUrl { get; set; }
        public IEnumerable<EquipmentDto> VenueDevices { get; set; } // 设备信息
        public IEnumerable<VenueMaintenanceDto> MaintenanceRecords { get; set; } // 保养记录
    }

    public class VenueAvailabilityDto
    {
        public int RemainingCapacity { get; set; } // 剩余容量
        public decimal Price { get; set; } // 场地价格
    }

    public class EquipmentDto
    {
        public string EquipmentId { get; set; } // 设备ID
        public string EquipmentName { get; set; } // 设备名称
    }

        public class VenueAnnouncementDto
    {
        public string VenueId { get; set; } // 场地ID
        public string Name { get; set; } // 场地名称
        public string Type { get; set; } // 运动类型
        public string VenueDescription { get; set; } // 场地状态
        public VenueAdminDto VenueAdminDto { get; set; } // 开放时间信息
        public List<VenueAnnouncementVDto> VenueAnnouncementsDto { get; set; } // 设备信息
    }
    public class VenueAdminDto
    {
        public string AdminId { get; set; }
        public string RealName { get; set; }
    }

    public class VenueAnnouncementVDto
    {
        public string AnnouncementId { get; set; } // 公告ID
        public string Title { get; set; }   // 公告标题
        public DateTime PublishedDate { get; set; } // 发布时间
        public DateTime LastModifiedDate { get; set; } // 最近修改时间
    }
}