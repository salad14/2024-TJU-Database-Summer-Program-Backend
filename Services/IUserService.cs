using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IUserService
    {
        RegisterResult Register(UserDto userDto);
        // LoginResult AuthenticateByUsername(string username, string password);
        // LoginResult AuthenticateByUserId(string userId, string password);
        LoginResult AuthenticateUser(string mode, string userInfo, string password);
        public string GenerateJwtToken(string userId, string userName, string userType);

        int GetUserViolationCount(string userId);
        User GetUserById(string userId);
        UserGroupInfoDto GetUserGroupInfo(string userId);

        UpdateResult UpdateUserInfo(string userId, string username, string contactNumber, string realName);
        UpdateResult UpdateUserPassword(string userId, string newPassword);

        IEnumerable<UserNotificationDto> GetUserNotifications(string userId);

        UpdateResult DeleteUserNotification(string noticeId);
    }
}
