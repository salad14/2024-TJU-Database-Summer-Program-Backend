namespace VenueBookingSystem.Models
{
    public class DeleteAvailabilityResult
    {
        public int State { get; set; } // 0为删除失败，1为删除成功
        public string Info { get; set; } // 返回操作结果的说明
    }
}
