using System;
using System.Collections.Generic;

namespace VenueBookingSystem.Models
{
    public class User
    {
        public required string UserId { get; set; } // 用户ID
        public required string Username { get; set; } // 用户名
        public required string Password { get; set; } // 密码
        public required string ContactNumber { get; set; } // 联系电话
        public required string UserType { get; set; } // 用户类型 (如：普通用户、管理员等)
        public bool ReservationPermission { get; set; } // 预约权限
        public int ViolationCount { get; set; } = 0; // 违约次数，默认值为0
        public bool IsVip { get; set; } // VIP状态
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // 注册时间
        public double DiscountRate { get; set; } = 0.0; // 折扣力度，默认值为0.0

         public required string RealName { get; set; }  // 真实姓名

        // 导航属性：用户的预约记录 (多对多)
        public ICollection<UserReservation>? UserReservations { get; set; } // 通过 UserReservation 表建立多对多关系

        // 导航属性：用户所属的团体 (多对多)
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    }

    public class Admin
    {
        public required string AdminId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }


}
