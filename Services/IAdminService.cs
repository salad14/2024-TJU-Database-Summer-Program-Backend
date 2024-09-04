<<<<<<< HEAD
using VenueBookingSystem.Dto;
=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IAdminService
    {
        // 定义接口方法，例如：
        Admin GetAdminById(string adminId);
        void CreateAdmin(Admin admin);
        IEnumerable<object> GetPublicNoticeData(string adminId);
<<<<<<< HEAD

        RegisterResult RegisterAdmin(AdminDto adminDto, List<string> manageVenues);
=======
>>>>>>> 888958e29fe44c188a51fa8e735f1492dd40ae99
        // 其他方法...
    }
}
