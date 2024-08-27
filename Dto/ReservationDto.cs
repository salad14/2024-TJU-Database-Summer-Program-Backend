namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
       public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal PaymentAmount { get; set; }  
        public string UserId { get; set; }  
        public string VenueId { get; set; }  
        public string AvailabilityId { get; set; }  
        public string ReservationItem { get; set; }  
        public int NumOfPeople { get; set; }  
    }
}
