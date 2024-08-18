namespace VenueBookingSystem.Models
{
    // 定义 VenueDto 类
    public class VenueDto
    {
        public required string Name { get; set; }  // 场地名称
        public required string Type { get; set; }  // 场地类型
        public int Capacity { get; set; }  // 场地容纳人数
    }
}
