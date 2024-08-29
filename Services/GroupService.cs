using VenueBookingSystem.Models;
using VenueBookingSystem.Data;
using System.Collections.Generic;
using System.Linq;
using VenueBookingSystem.Dto;
using Microsoft.EntityFrameworkCore;

namespace VenueBookingSystem.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<GroupUser> _groupUserRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserNotification> _userNotificationRepository;

        private readonly ApplicationDbContext _context;
        public GroupService(IRepository<Group> groupRepository, IRepository<GroupUser> groupUserRepository, IRepository<User> userRepository, IRepository<UserNotification> userNotificationRepository, ApplicationDbContext context)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _userRepository = userRepository;
            _userNotificationRepository = userNotificationRepository;
            _context = context;
        }



        // 修改 CreateGroup 方法以返回 GroupCreateResult
        public GroupCreateResult CreateGroup(GroupDto groupDto)
        {
            // 检查团体名称是否唯一
            if (_groupRepository.Find(g => g.GroupName == groupDto.GroupName).Any())
            {
                return new GroupCreateResult
                {
                    State = 0,
                    GroupId = null,
                    Info = "团体名称已存在"
                };
            }

            // 生成唯一的团体ID
            string groupId = GenerateUniqueGroupId();

            // 创建团体实体
            var group = new Group
            {
                GroupId = groupId,
                GroupName = groupDto.GroupName,
                Description = groupDto.Description,
                CreatedDate = DateTime.UtcNow
            };

            // 添加团体到数据库
            _groupRepository.Add(group);

            return new GroupCreateResult
            {
                State = 1,
                GroupId = group.GroupId,
                Info = "团体创建成功"
            };
        }

        public Group GetGroupById(string groupId)
        {
            return _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
        }

        public IEnumerable<Group> GetAllGroups()
        {
            return _groupRepository.GetAll().ToList();
        }

        public GroupAddResult AddUserToGroup(string groupId, string userId, DateTime joinDate, string roleInGroup)
        {
            var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
            var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();

            if (group == null)
            {
                return new GroupAddResult
                {
                    State = 0,
                    Info = "未找到指定的团体"
                };
            }

            if (user == null)
            {
                return new GroupAddResult
                {
                    State = 0,
                    Info = "未找到指定的用户"
                };
            }

            var existingMembership = _groupUserRepository.Find(gu => gu.GroupId == groupId && gu.UserId == userId).FirstOrDefault();
            if (existingMembership != null)
            {
                return new GroupAddResult
                {
                    State = 0,
                    Info = "用户已加入该团体"
                };
            }

            var groupUser = new GroupUser 
            { 
                GroupId = groupId, 
                UserId = userId, 
                Group = group, 
                User = user,
                JoinDate = joinDate,
                RoleInGroup = roleInGroup
            };

            _groupUserRepository.Add(groupUser);
            group.MemberCount++;
            _groupRepository.Update(group); // 更新成员数量

            // 生成通知
            var notification = new UserNotification
            {
                UserId = userId,
                NotificationId = GenerateNotificationId(), // 生成唯一的通知ID
                NotificationType = "Group Operation Notification",  // 修改为英文，避免字符集问题
                Title = "Group Join Confirmation",  // 修改为英文，避免字符集问题
                Content = $"Administrator [{userId}] has added you to the group [{group.GroupName}]", // 修改为英文，避免字符集问题
                NotificationTime = DateTime.UtcNow
            };


            Console.WriteLine(notification.Content);
            _userNotificationRepository.Add(notification);

            return new GroupAddResult
            {
                State = 1,
                Info = ""
            };
        }

        public GroupRemoveResult RemoveUserFromGroup(string groupId, string userId, string adminId)
        {
            var groupUser = _groupUserRepository.Find(gu => gu.GroupId == groupId && gu.UserId == userId).FirstOrDefault();
            if (groupUser == null)
            {
                return new GroupRemoveResult
                {
                    State = 0,
                    Info = "未找到该用户的团体记录"
                };
            }

            _groupUserRepository.Delete(groupUser);

            var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
            if (group != null)
            {
                group.MemberCount--;
                _groupRepository.Update(group); // 更新成员数量
            }

            // 生成通知
            var notification = new UserNotification
            {
                UserId = userId,
                NotificationId = GenerateNotificationId(),
                NotificationType = "Group Notification",  // Modify as needed
                Title = "Group Exit",
                Content = string.IsNullOrEmpty(adminId) 
                    ? $"You have successfully exited the group [{group?.GroupName}]" 
                    : $"You have been removed from the group [{group?.GroupName}] by the administrator [{adminId}]",
                NotificationTime = DateTime.UtcNow
            };


            _userNotificationRepository.Add(notification);

            return new GroupRemoveResult
            {
                State = 1,
                Info = ""
            };
        }

        public GroupUpdateResult UpdateUserRoleInGroup(string groupId, string userId, string userRole, string adminId)
        {
            var groupUser = _groupUserRepository.Find(gu => gu.GroupId == groupId && gu.UserId == userId).FirstOrDefault();

            if (groupUser == null)
            {
                return new GroupUpdateResult
                {
                    State = 0,
                    Info = "未找到指定的用户或团体记录"
                };
            }

            // 更新用户团体地位
            groupUser.RoleInGroup = userRole;
            _groupUserRepository.Update(groupUser);

            // 获取团体信息
            var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();

            // 生成通知
            var notification = new UserNotification
            {
                UserId = userId,
                NotificationId = GenerateNotificationId(),  // 生成唯一的通知ID
                NotificationType = "Group Notification",  // 通知类型
                Title = "Group Role Change",  // 通知标题
                Content = $"Administrator [{adminId}] has updated your role in the group [{group?.GroupName}] to [{userRole}]",
                NotificationTime = DateTime.UtcNow
            };

            _userNotificationRepository.Add(notification);

            return new GroupUpdateResult
            {
                State = 1,
                Info = ""
            };
        }

        // 查找某一用户归属的所有团体
        public IEnumerable<UserGroupDto> UserAllGroups(string userId)
        {
            var groupUsers = _context.GroupUsers
            .Include(gu => gu.Group) // 使用 Include 加载 Group 实体
            .Where(gu => gu.UserId == userId)
            .ToList();

            var userGroups = groupUsers
                .Where(gu => gu.Group != null)
                .Select(gu => new UserGroupDto
                {
                    GroupId = gu.GroupId,
                    GroupName = gu.Group.GroupName,
                    Description = gu.Group.Description,
                    MemberCount = gu.Group.MemberCount,
                    CreatedDate = gu.Group.CreatedDate,
                    JoinDate = gu.JoinDate,
                    RoleInGroup = gu.RoleInGroup
                }).ToList();

            return userGroups;
        }


        // 查找所有不同ID的团体
        public IEnumerable<Group> SelectGroups()
        {
            return _groupRepository.GetAll().GroupBy(g => g.GroupId).Select(g => g.First()).ToList();
        }

        // 生成唯一的团体ID
        private string GenerateUniqueGroupId()
        {
            var random = new Random();
            string groupId;

            do
            {
                groupId = random.Next(100000, 999999).ToString();
            } while (_groupRepository.Find(g => g.GroupId == groupId).Any()); // 确保ID唯一性

            return groupId;
        }

        // 生成唯一的通知ID
        private string GenerateNotificationId()
        {
            // 获取当前数据库中最大的通知ID
            var maxNotificationId = _userNotificationRepository.GetAll()
                .Select(n => Convert.ToInt32(n.NotificationId))
                .DefaultIfEmpty(0) // 如果没有记录，则返回0
                .Max();

            // 生成下一个通知ID
            int newNotificationId = maxNotificationId + 1;

            // 返回新的通知ID，转换为字符串
            return newNotificationId.ToString();
        }


    }
}
