namespace VenueBookingSystem.Models
{
    public class UserDto
    {
        public string? UserId { get; set; }  // 注意这里的 ? 符号表示该字段是可选的
        public required string Username { get; set; }  // 用户名
        public required string Password { get; set; }  // 密码
        public required string ContactNumber { get; set; }  // 联系电话
        public required string UserType { get; set; }  // 用户类型
        public string? ReservationPermission { get; set; }  // 预约权限
        public int ViolationCount { get; set; } = 0;  // 违约次数
        public string? IsVip { get; set; } // VIP状态
        public double DiscountRate { get; set; } = 0.0;  // 折扣力度
        public required string RealName { get; set; }  // 真实姓名
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // 注册时间
    }

    public class UserGroupInfoDto
    {
        public DateTime JoinDate { get; set; } // 加入时间
        public string RoleInGroup { get; set; } // 团体中的地位
        public string GroupId { get; set; } // 团体ID
        public string? GroupName { get; set; } // 团体名称
    }

    public class UserNotificationDto
    {
        public string NotificationId { get; set; }  // 通知ID
        public string UserId { get; set; }  // 用户ID
        public string NotificationType { get; set; }  // 通知类型 (例如 'reservation')
        public string Title { get; set; }  // 通知标题
        public string Content { get; set; }  // 通知内容
        public DateTime NotificationTime { get; set; }  // 通知时间

        public string TargetTeam { get; set; }

        public string TargetUser { get; set; }

    }

    public class UserGroupDetailDto
    {
        public string UserId { get; set; } // 用户ID
        public string UserRole { get; set; } // 用户团体地位
        public string UserName { get; set; } // 用户名
    }

    public class UpdateUserInfoDto
    {
        public string Username { get; set; }
        public string ContactNumber { get; set; }
        public string RealName { get; set; }
    }

    public class UpdateUserPasswordDto
    {
        public string NewPassword { get; set; }
    }

}
