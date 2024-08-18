using VenueBookingSystem.Models;
using VenueBookingSystem.Data;

namespace VenueBookingSystem.Services
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _venueRepository;

        // 构造函数，注入存储库
        public VenueService(IRepository<Venue> venueRepository)
        {
            _venueRepository = venueRepository;
        }

        // 获取所有场地
        public IEnumerable<Venue> GetAllVenues()
        {
            return _venueRepository.GetAll();
        }

        // 添加新场地
        public void AddVenue(VenueDto venueDto)
        {
            var venue = new Venue
            {
                Name = venueDto.Name,
                Type = venueDto.Type,
                Capacity = venueDto.Capacity
            };
            _venueRepository.Add(venue);
        }

        // 其他场地服务逻辑...
    }
}
