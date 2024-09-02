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
                NotificationType = "team/join",  // 通知类型
                Title = "加入团体确认",  // 通知标题
                Content = $"管理员 [{userId}] 已将您加入团体 [{group.GroupName}]",
                NotificationTime = DateTime.UtcNow
            };



            Console.WriteLine(notification.Content);
            _userNotificationRepository.Add(notification);

            return new GroupAddResult
            {
                State = 1,
                Info = "加入成功"
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
                NotificationType = "team/quit",  // 根据实际需求可以修改
                Title = "退出团体",
                Content = string.IsNullOrEmpty(adminId) 
                    ? $"您已成功退出团体 [{group?.GroupName}]" 
                    : $"您已被管理员 [{adminId}] 移出团体 [{group?.GroupName}]",
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
                NotificationType = "team/roleChange",  // 通知类型
                Title = "团体角色变更",  // 通知标题
                Content = $"管理员 [{adminId}] 已将您在团体 [{group?.GroupName}] 中的角色更新为 [{userRole}]",
                NotificationTime = DateTime.UtcNow
            };

            _userNotificationRepository.Add(notification);

            return new GroupUpdateResult
            {
                State = 1,
                Info = "变更成功"
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

        public GroupDetailDto GetGroupDetailById(string groupId)
        {
            // 获取团体信息
            var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
            if (group == null)
            {
                return null;
            }

            // 获取团体成员信息
            var userGroupDetails = from gu in _context.GroupUsers
                                join u in _context.Users on gu.UserId equals u.UserId
                                where gu.GroupId == groupId
                                select new UserGroupDetailDto
                                {
                                    UserId = gu.UserId,
                                    UserRole = gu.RoleInGroup,
                                    UserName = u.Username
                                };

            return new GroupDetailDto
            {
                Description = group.Description,
                CreatedDate = group.CreatedDate,
                Users = userGroupDetails.ToList()
            };
        }

        public bool UpdateGroupInfo(string groupId, string groupName, string description)
        {
            var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null)
            {
                return false; // 团体不存在，返回 false
            }

            // 更新团体信息
            group.GroupName = groupName;
            group.Description = description;

            _context.Groups.Update(group);
            _context.SaveChanges();

            return true; // 更新成功，返回 true
        }



    }
}
