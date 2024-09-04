using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;
using System.Linq;

namespace VenueBookingSystem.Services
{
    public interface IUserPersonalInfoService
    {
        UserPersonalInfoDto GetUserPersonalInfo(string userId);
        List<UserIdNameDto> GetAllUserIdsAndNames();
    }

    public class UserPersonalInfoService : IUserPersonalInfoService
    {
        private readonly IRepository<User> _userRepository;

        public UserPersonalInfoService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public UserPersonalInfoDto GetUserPersonalInfo(string userId)
        {
            var user = _userRepository.Find(u => u.UserId == userId)
                .Select(u => new UserPersonalInfoDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    ContactNumber = u.ContactNumber,
                    ViolationCount = u.ViolationCount,
                    ReservationPermission = u.ReservationPermission
                })
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("用户未找到");
            }

            return user;
        }

        public List<UserIdNameDto> GetAllUserIdsAndNames()
    {
        var users = _userRepository.GetAll()
            .Select(u => new UserIdNameDto
            {
                UserId = u.UserId,
                Username = u.Username
            })
            .ToList();

        return users;
    }
    }
}