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
}

