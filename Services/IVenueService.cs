using VenueBookingSystem.Models;

namespace VenueBookingSystem.Services
{
    // 定义 IVenueService 接口
    public interface IVenueService
    {
        // 获取所有场地的方法签名
        IEnumerable<Venue> GetAllVenues();

        // 添加新场地的方法签名
        void AddVenue(VenueDto venueDto);
    }
}
