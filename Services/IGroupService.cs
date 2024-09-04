using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;
using System.Collections.Generic;

namespace VenueBookingSystem.Services
{
    public interface IGroupService
    {
        GroupCreateResult CreateGroup(GroupDto groupDto);
        Group GetGroupById(string groupId);
        IEnumerable<Group> GetAllGroups();
        GroupAddResult AddUserToGroup(string groupId, string userId, DateTime joinDate, string roleInGroup);

        GroupRemoveResult RemoveUserFromGroup(string groupId, string userId, string adminId);

        GroupUpdateResult UpdateUserRoleInGroup(string groupId, string userId, string userRole, string adminId);

<<<<<<< HEAD
        GroupDetailDto GetGroupDetailById(string groupId);

        bool UpdateGroupInfo(string groupId, string groupName, string description); 

=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        // 查找某一用户归属的所有团体
        IEnumerable<UserGroupDto> UserAllGroups(string userId);

        // 查找所有不同ID的团体
        IEnumerable<Group> SelectGroups();
    }
}
