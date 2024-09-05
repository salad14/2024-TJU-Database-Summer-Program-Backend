using System.ComponentModel.DataAnnotations;
namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public decimal PaymentAmount { get; set; }  // 支付金额
        public string VenueId { get; set; }  // 场地ID
        public string AvailabilityId { get; set; }  // 开放时间段ID
        public string ReservationItem { get; set; }  // 预约项目 (比如租借的设备或场地)
        public required string ReservationType { get; set; }  // 预约类型，必填字段

        public int NumOfPeople { get; set; } 
    }

    public class UserReservationDto
    {
        public string UserId { get; set; }  // 用户ID
        public string ReservationId { get; set; }  // 预约ID (来自 `Reservations` 表)
        public int NumOfPeople { get; set; }  // 预约人数
        public string Status { get; set; } = "已预约";  // 默认预约状态
        public DateTime? CheckInTime { get; set; }  // 签到时间 (可以为null)
    }

    public class GroupReservationDto
    {
        public string GroupId { get; set; }  // 团体ID
        public string GroupName { get; set; }  // 团体ID
        public decimal PaymentAmount { get; set; }  // 支付金额
        public string VenueId { get; set; }  // 场地ID
        public string AvailabilityId { get; set; }  // 开放时间段ID
        public string ReservationType { get; set; }  // 预约类型，团体
        public string ReservationItem { get; set; }
        public List<string> UserIds { get; set; }  // 用户ID数组
    }

    public class AvailabilityDto
    {
        public string AvailabilityId { get; set; } // 开放时间段ID
        public DateTime StartTime { get; set; } // 开放开始时间
        public DateTime EndTime { get; set; } // 开放结束时间
        public decimal Price { get; set; } // 场地价格
        public int RemainingCapacity { get; set; } // 剩余容量
    }

    public class ReservationUserDetailDto
    {
        public  string UserId { get; set; } // 用户ID
        public DateTime? CheckInTime { get; set; } // 用户签到时间
        public  string Status { get; set; } // 预约状态
        public string Username { get; set; } // 用户名

        public string RealName { get; set; }  // 真实姓名
    }

    public class UpdateReservationUserDto
    {
        
        public string ReservationId { get; set; } // 用户ID
        public string UserId { get; set; } // 用户ID
        public DateTime? CheckInTime { get; set; } // 用户签到时间
        public string Status { get; set; } // 预约状态
    }

    public class ReservationListDto
    {
        public  string ReservationId { get; set; } // 预约ID
        public  string VenueId { get; set; } // 场地ID
        public  string AvailabilityId { get; set; } // 开放时间段ID
        public  string ReservationType { get; set; }  //预约类型（用户，团体）

        public  DateTime ReservationTime { get; set; } // 预约操作时间
        public  decimal PaymentAmount { get; set; } // 支付金额

        public int NumOfPeople { get; set; }

        public string VenueName { get; set; } // 场地名称
        public  DateTime StartTime { get; set; } // 开始时间
        public  DateTime EndTime { get; set; } // 结束时间
        public List<GroupReservationListDto> GroupReservationListDto { get; set; }
    }

    public class GroupReservationListDto
    {
        public  string GroupId { get; set; } // 团体ID

        public  string GroupName { get; set; } // 团体名称
    }

    public class ReservationVenueListDto
    {
        public string ReservationId { get; set; } // 预约ID
        public string VenueId { get; set; } // 场地ID
        public string AvailabilityId { get; set; } // 开放时间段ID
        public string ReservationType { get; set; }  //预约类型（用户，团体）

        public DateTime ReservationTime { get; set; } // 预约操作时间
        public decimal PaymentAmount { get; set; } // 支付金额

        public int NumOfPeople { get; set; }

        public string VenueName { get; set; } // 场地名称
       
    }

    public class ReservationStatusUpdateDto
    {
        [Required]
        public string ReservationId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime CheckInTime { get; set; }

        [Required]
        public string Status { get; set; }
    }


    public class ReservationDetailDto
    {
        public string ReservationId { get; set; }
        public string VenueId { get; set; }
        public string VenueName { get; set; }
        public string AvailabilityId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReservationTime { get; set; }
        public decimal PaymentAmount { get; set; }
        public string ReservationType { get; set; }
        public int? NumOfPeople { get; set; } // 针对个人预约
        public int? MemberCount { get; set; } // 针对团体预约
        public List<UserDetailDto> UserDetails { get; set; }
        public GroupReservationDetailDto GroupDetails { get; set; }
    }

    public class UserDetailDto
    {
        public string UserId { get; set; }
        public string RealName { get; set; }
    }

    public class GroupReservationDetailDto
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
    }


    public class VenueIdsDto
    {
        public List<string> VenueIds { get; set; }
    }

    public class UserReservationInfoDto
    {
        public string UserId { get; set; } // 用户ID
        public string Username { get; set; } // 用户名
        public string RealName { get; set; } // 真实姓名
        public DateTime? CheckInTime { get; set; } // 签到时间
        public string Status { get; set; } // 预约状态
    }
}

