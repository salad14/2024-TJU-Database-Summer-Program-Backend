namespace VenueBookingSystem.Models
{
    public class AvailabilityDto
    {
        public string AvailabilityId { get; set; } // 开放时间段ID
        public DateTime StartTime { get; set; } // 开放开始时间
        public DateTime EndTime { get; set; } // 开放结束时间
        public decimal Price { get; set; } // 场地价格
        public int RemainingCapacity { get; set; } // 剩余容量
    }
}
