using VenueBookingSystem.Models;

namespace VenueBookingSystem.Dto
{
    public class GroupDto
    {
        public string? GroupName { get; set; } // 团体名称
        public string Description { get; set; } // 团体描述
    }

    public class GroupDetailDto
    {
        public string Description { get; set; } // 团体描述
        public DateTime CreatedDate { get; set; } // 团体创建时间
        public List<UserGroupDetailDto> Users { get; set; } // 团队成员信息
    }

    public class UserGroupDto
    {
        public string? GroupId { get; set; } // 团体ID
        public string? GroupName { get; set; } // 团体名称
        public string? Description { get; set; } // 团体描述
        public int MemberCount { get; set; } // 团队人数
        public DateTime CreatedDate { get; set; } // 团体创建时间
        public DateTime JoinDate { get; set; } // 用户加入团体的时间
        public string? RoleInGroup { get; set; } // 用户在团体中的地位
    }

    public class UserJoinGroupDto
    {
        public required string UserId { get; set; }  // 用户ID
        public DateTime JoinDate { get; set; }  // 用户加入时间
        public required string RoleInGroup { get; set; }  // 用户团体地位
        public string? AdminId { get; set; }
        public required string NotificationType { get; set; }
        public string? UserName { get; set; }
    }

    public class RemoveUserDto
    {
        public required string UserId { get; set; }  // 用户ID
        public string? AdminId { get; set; }  // 管理员ID，如果为空表示用户自行退出
    }

    public class UpdateUserRoleDto
    {
        public required string UserId { get; set; }  // 用户ID
        public required string GroupId { get; set; }  // 团体ID
        public required string UserRole { get; set; }  // 用户团体地位
        public string? AdminId { get; set; }  // 管理员ID，可选
        public string? NotificationType { get; set; }
    }

    public class GroupUpdateDto
    {
        public required string GroupId { get; set; } // 团体ID
        public string? GroupName { get; set; } // 团体名称
        public string? Description { get; set; } // 团体描述
    }

}
