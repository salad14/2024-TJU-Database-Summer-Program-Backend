namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public decimal PaymentAmount { get; set; }  
        public string UserId { get; set; }  
        public string VenueId { get; set; }  
        public string AvailabilityId { get; set; }  
        public string ReservationItem { get; set; }  
        public int NumOfPeople { get; set; } 
        public required string ReservationType { get; set; }  
    }
}
