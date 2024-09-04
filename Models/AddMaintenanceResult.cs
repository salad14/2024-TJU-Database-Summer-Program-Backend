namespace VenueBookingSystem.Models
{
    public class AddMaintenanceResult
    {
        public int State { get; set; } // 0为添加失败，1为添加成功
        public string MaintenanceId { get; set; } // 保养记录ID，失败时为空
        public string Info { get; set; } // 返回操作结果的说明
    }
}
