using VenueBookingSystem.Models;
using VenueBookingSystem.Data;

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

        public void CreateGroup(Group group)
        {
            _groupRepository.Add(group);
        }

        public Group GetGroupById(int groupId)
        {
            return _groupRepository.Find(g => g.GroupId == groupId).FirstOrDefault();
        }


        public IEnumerable<Group> GetAllGroups()
        {
            return _groupRepository.GetAll().ToList();
        }

        public void AddUserToGroup(int groupId, string userId)
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

        public void RemoveUserFromGroup(int groupId, string userId)
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

        public void AddUserToGroup(int groupId, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromGroup(int groupId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
