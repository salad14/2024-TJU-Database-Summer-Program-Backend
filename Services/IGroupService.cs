using VenueBookingSystem.Models;
using System.Collections.Generic;

namespace VenueBookingSystem.Services
{
    public interface IGroupService
    {
        void CreateGroup(Group group);
        Group GetGroupById(int groupId);
        IEnumerable<Group> GetAllGroups();
        void AddUserToGroup(int groupId, int userId);
        void RemoveUserFromGroup(int groupId, int userId);
    }
}
