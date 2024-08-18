namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal AmountPaid { get; set; }
        public int UserId { get; set; }
        public int VenueId { get; set; }
    }
}
