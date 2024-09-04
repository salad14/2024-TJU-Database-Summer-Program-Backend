namespace VenueBookingSystem.Dto
{
    // 注册结果类
    public class RegisterResult
    {
        public int State { get; set; }
        public string? UserId { get; set; }
        public string? AdminId { get; set; }
        public string? Info { get; set; }
    }

    // 登录结果类
    public class LoginResult
    {
    public int State { get; set; }  // 登录状态，1 表示成功，0 表示失败
    public required string UserId { get; set; }  // 用户ID
    public required string UserName { get; set; }  // 用户名
    public required string UserType { get; set; }  // 用户类型
    public required string Info { get; set; }  // 额外的信息，通常用于存储错误消息
    }

    //创建团队结果类
    public class GroupCreateResult
    {
        public int State { get; set; }  // 创建操作的结果：0为创建失败，1为创建成功
        public string? GroupId { get; set; }  // 该团体分配的ID
        public string Info { get; set; } = "";  // 创建结果的说明
    }

    //用户加入团体结果
    public class GroupAddResult
    {
        public int State { get; set; }  // 0为加入失败，1为加入成功
        public string Info { get; set; }  // 加入结果的说明
    }
    //用户退出团体结果
    public class GroupRemoveResult
    {
        public int State { get; set; }  // 0为删除失败，1为删除成功
        public string Info { get; set; }  // 删除结果的说明
    }

        public class GroupUpdateResult
    {
        public int State { get; set; }  // 状态：0表示失败，1表示成功
        public string Info { get; set; }  // 信息：成功时为空字符串，失败时包含错误信息
    }

    public class UpdateResult
    {
        public int State { get; set; }  // 0 失败，1 成功
        public string Info { get; set; } // 信息
    }

    public class AddAnnouncementResult
    {
        public int State { get; set; } // 0为失败，1为成功
        public string AnnouncementId { get; set; } // 返回生成的公告ID
        public string Info { get; set; } // 成功时为空字符串，失败时返回原因
    }

    public class UpdateAnnouncementResult
    {
        public int State { get; set; } // 0 表示失败，1 表示成功
        public string AnnouncementId { get; set; } // 更新的公告ID
        public string Info { get; set; } // 错误或成功信息
    }





}
