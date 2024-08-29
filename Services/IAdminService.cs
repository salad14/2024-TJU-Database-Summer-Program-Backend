using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    public interface IAdminService
    {
        // 定义接口方法，例如：
        Admin GetAdminById(string adminId);
        void CreateAdmin(Admin admin);
        IEnumerable<object> GetPublicNoticeData(string adminId);
        // 其他方法...
    }
}
