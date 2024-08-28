using VenueBookingSystem.Models;
using VenueBookingSystem.Data;
using System.Collections.Generic;
using System.Linq;
using VenueBookingSystem.Dto;  // 确保导入正确的命名空间

namespace VenueBookingSystem.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<GroupUser> _groupUserRepository;
        private readonly IRepository<User> _userRepository;

        public GroupService(IRepository<Group> groupRepository, IRepository<GroupUser> groupUserRepository, IRepository<User> userRepository)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _userRepository = userRepository;
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

        public void AddUserToGroup(string groupId, string userId)
        {
            var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
            var user = _userRepository.Find(u => u.UserId == userId).FirstOrDefault();
            if (group != null && user != null)
            {
                var groupUser = new GroupUser { GroupId = groupId, UserId = userId, Group = group, User = user };
                _groupUserRepository.Add(groupUser);
                group.MemberCount++;
                _groupRepository.Update(group); // 更新成员数量
            }
        }

        public void RemoveUserFromGroup(string groupId, string userId)
        {
            var groupUser = _groupUserRepository.Find(gu => gu.GroupId == groupId && gu.UserId == userId).FirstOrDefault();
            if (groupUser != null)
            {
                _groupUserRepository.Delete(groupUser);
                var group = _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
                if (group != null)
                {
                    group.MemberCount--;
                    _groupRepository.Update(group); // 更新成员数量
                }
            }
        }

        // 查找某一用户归属的所有团体
        public IEnumerable<Group> UserAllGroups(string userId)
        {
            var groupIds = _groupUserRepository.Find(gu => gu.UserId == userId).Select(gu => gu.GroupId).ToList();
            return _groupRepository.Find(g => groupIds.Contains(g.GroupId)).ToList();
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
    }
}
