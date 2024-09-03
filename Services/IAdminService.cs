using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IAdminService
    {
        // 定义接口方法，例如：
        Admin GetAdminById(string adminId);
        void CreateAdmin(Admin admin);
        IEnumerable<object> GetAdminNoticeData(string adminId);

        UpdateResult UpdateAdminInfo(string adminId, AdminUpdateDto adminUpdateDto, List<string> manageVenues);
        
        UpdateResult UpdateAdminPassword(string adminId, string newPassword);

        RegisterResult RegisterAdmin(AdminDto adminDto, List<string> manageVenues,string? systemAdminId = null);
        // 其他方法...
    }
}
