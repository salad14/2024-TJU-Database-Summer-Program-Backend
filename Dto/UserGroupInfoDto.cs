namespace VenueBookingSystem.Dto
{
    // 用户团体信息 DTO
    public class UserPersonalGroupInfoDto
    {
        public string GroupId { get; set; } // 团体ID
        public string GroupName { get; set; } // 团体名称
        public string GroupDescription { get; set; } // 团体描述
        public int MemberCount { get; set; } // 团体人数
        public DateTime CreatedDate { get; set; } // 创建时间
    }
}