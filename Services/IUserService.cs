using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IUserService
    {
        RegisterResult Register(UserDto userDto);
        LoginResult AuthenticateByUsername(string username, string password);
        LoginResult AuthenticateByUserId(string userId, string password);

        public string GenerateJwtToken(string userId, string userName, string userType);

        User GetUserById(string userId);
        UserGroupInfoDto GetUserGroupInfo(string userId);

        IEnumerable<UserNotificationDto> GetUserNotifications(string userId);
    }
}
