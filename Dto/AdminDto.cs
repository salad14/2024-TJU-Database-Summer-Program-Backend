namespace VenueBookingSystem.Models
{

    public class AdminDto
    {
        public string RealName { get; set; }  // 管理员真实姓名
        public string? Password { get; set; }  // 管理员密码（已加密）
        public string ContactNumber { get; set; }  // 管理员联系电话
        public string AdminType { get; set; }  // 管理员类型，如 admin-validate/system
        public string? ApplyDescription { get; set; } // 申请说明
    }

    public class AdminResponseDto
    {
        public string RealName { get; set; }
        public string ContactNumber { get; set; }
        public string AdminType { get; set; }
    }


    public class AdminRequestDto
    {
        public string AdminId { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
        public string? AdminType { get; set; }
    }

    public class AdminNotificationDto
    {
        public required string NotificationId { get; set; } // 通知ID
        public string NotificationType { get; set; } // 通知类型
        public string Title { get; set; } // 通知标题
        public string Content { get; set; } // 通知内容
        public DateTime NotificationTime { get; set; } // 通知时间
        public string NewAdminId { get; set; } // 新管理员ID
    }

    public class AdminUpdateDto
    {
        public string RealName { get; set; } // 管理员真实姓名
        public string ContactNumber { get; set; } // 管理员联系电话
        public string AdminType { get; set; } // 管理员类型
    }

    public class UpdateAdminPasswordDto
    {
        public string NewPassword { get; set; } // 新的管理员密码
    }

    public class AdminManagedItemsDto
    {
        public IEnumerable<VenueDto> ManagedVenues { get; set; }
        public IEnumerable<EquipmentDto> ManagedEquipment { get; set; }
    }

    public class AdminRegistrationDto
    {
        public AdminDto AdminDto { get; set; }
        public List<string> ManageVenues { get; set; }

        public string? SystemAdminId { get; set; }
    }

    public class AdminInfoResultDto
    {
        public int State { get; set; }
        public string Info { get; set; }
        public AdminInfoDto Data { get; set; }
    }

    public class AdminInfoDto
    {
        public string RealName { get; set; }
        public string ContactNumber { get; set; }
        public string AdminType { get; set; }
    }
    
}
