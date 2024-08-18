using System;
using System.Collections.Generic;

namespace VenueBookingSystem.Models
{
    public class User
    {
        public int UserId { get; set; } // 用户ID
        public required string Username { get; set; } // 用户名
        public required string Password { get; set; } // 密码
        public required string ContactNumber { get; set; } // 联系电话
        public bool IsVip { get; set; } // 是否为VIP用户
        public DateTime RegistrationDate { get; set; } // 注册时间

        // 导航属性：用户的预约记录
         public ICollection<Reservation>? Reservations { get; set; }
    }
}
