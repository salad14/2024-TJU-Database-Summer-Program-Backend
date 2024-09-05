using VenueBookingSystem.Data;
using VenueBookingSystem.Models;

namespace sports_management.Services
{
    public class VenueAvailabilityService : IVenueAvailabilityServices
    {
        private readonly IRepository<VenueAvailability> _venueAvailabilityRepository;
        public VenueAvailabilityService(IRepository<VenueAvailability> venueAvailabilityRepository)
        {
            _venueAvailabilityRepository = venueAvailabilityRepository;
        }

        //通过请求的时间获取给定时间内开放的所有场地
        public List<VADto> GetVenueAvailability(string dateReq)
        {

            List<VADto> vaList = new List<VADto>();
           var list = _venueAvailabilityRepository.GetAll().Where(x => x.StartTime > Convert.ToDateTime(dateReq) && x.EndTime <= Convert.ToDateTime(dateReq).AddDays(1));
            if(list.Any())
            {
                foreach(var item in list)
                {
                    var  venue = new VADto ();
                    venue.VenueId = item?.Venue.VenueId;
                    venue.Name = item?.Venue.Name;
                    venue.Capacity = item.Venue==null?0: item.Venue.Capacity;
                    venue.Type = item?.Venue.Type;
                    vaList.Add(venue);
                }
            }
            return vaList;
        } 
    }
}
