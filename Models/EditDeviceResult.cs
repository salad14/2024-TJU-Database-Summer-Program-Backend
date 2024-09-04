namespace VenueBookingSystem.Models
{
    public class EditDeviceResult
    {
        public int State { get; set; } // 0为修改失败，1为修改成功
        public string Info { get; set; } // 返回操作结果的说明
    }
}
