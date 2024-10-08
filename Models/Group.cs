using System;
using System.Collections.Generic;

namespace VenueBookingSystem.Models
{
    public class Group
    {
        public required string GroupId { get; set; } // 团体ID
        public required string GroupName { get; set; } // 团体名称
        public string? Description { get; set; } // 团体描述
        public int MemberCount { get; set; } = 0; // 团队人数，初始为0
        public DateTime CreatedDate { get; set; } = DateTime.Now; // 创建时间

       // 导航属性：团体的成员
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        // 导航属性：团体的预约记录 (一对多)
        public ICollection<GroupReservationMember>? GroupReservations { get; set; } // 可空，ORM 将填充
    }
}
