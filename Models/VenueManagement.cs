namespace VenueBookingSystem.Models
{
    public class VenueManagement
    {
        public required string VenueId { get; set; } // 场地ID，关联的场地唯一标识符
        public required string AdminId { get; set; } // 管理员ID，负责管理场地的管理员的唯一标识符
    
        // 导航属性：关联的场地和管理员
        public Venue Venue { get; set; } // 关联的场地
        public Admin Admin { get; set; } // 关联的管理员
    }
}
