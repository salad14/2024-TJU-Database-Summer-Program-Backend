using VenueBookingSystem.Dto;
using System;

namespace VenueBookingSystem.Services
{
    public interface IVenueAnalysisService
    {
        VenueAnalysisDto GetVenueAnalysisData(string venueType, DateTime? currentTime = null);
        VenueAnalysisDto GetSingleVenueAnalysisData(string venueId, DateTime? currentTime = null);
    }
}
