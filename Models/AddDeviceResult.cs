namespace VenueBookingSystem.Models
{
    public class AddDeviceResult
    {
        public int State { get; set; } // 0为添加失败，1为添加成功
        public string DeviceId { get; set; } // 设备ID，失败时为空
        public string Info { get; set; } // 添加结果的说明，成功时为空，失败时说明失败原因
    }
}
