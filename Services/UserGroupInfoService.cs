using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace VenueBookingSystem.Services
{
    public interface IUserGroupInfoService
    {
        List<UserPersonalGroupInfoDto> GetUserGroups(string userId);
    }

    public class UserGroupInfoService : IUserGroupInfoService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<GroupUser> _groupUserRepository;

        public UserGroupInfoService(IRepository<Group> groupRepository, IRepository<GroupUser> groupUserRepository)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
        }

        public List<UserPersonalGroupInfoDto> GetUserGroups(string userId)
        {
            var userGroups = (from gu in _groupUserRepository.Find(gu => gu.UserId == userId && (gu.RoleInGroup == "Admin" || gu.RoleInGroup == "Creator"))
                              join g in _groupRepository.GetAll() on gu.GroupId equals g.GroupId
                              select new UserPersonalGroupInfoDto
                              {
                                  GroupId = g.GroupId,
                                  GroupName = g.GroupName,
                                  GroupDescription = g.Description,
                                  MemberCount = g.GroupUsers.Count(),
                                  CreatedDate = g.CreatedDate
                              }).ToList();

            return userGroups;
        }
    }
}