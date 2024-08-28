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
        void AddUserToGroup(string groupId, string userId);
        void RemoveUserFromGroup(string groupId, string userId);

        // 查找某一用户归属的所有团体
        IEnumerable<Group> UserAllGroups(string userId);

        // 查找所有不同ID的团体
        IEnumerable<Group> SelectGroups();
    }
}
