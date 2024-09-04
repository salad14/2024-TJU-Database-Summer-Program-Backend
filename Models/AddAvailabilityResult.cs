namespace VenueBookingSystem.Models
{
    public class AddAvailabilityResult
    {
        public int State { get; set; } // 0为添加失败，1为添加成功
        public string AvailabilityId { get; set; } // 开放时间段ID，失败时为空
        public string Info { get; set; } // 返回操作结果的说明
    }
}
