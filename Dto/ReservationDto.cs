namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public decimal PaymentAmount { get; set; }  
        public string UserId { get; set; }  
        public string VenueId { get; set; }  
        public string AvailabilityId { get; set; }  
        public string ReservationItem { get; set; }  
        
        public required string ReservationType { get; set; }  
    }



    public class UserReservationInfoDto
    {
        public string UserId { get; set; } // �û�ID
        public string Username { get; set; } // �û���
        public string RealName { get; set; } // ��ʵ����
        public DateTime? CheckInTime { get; set; } // ǩ��ʱ��
        public string Status { get; set; } // ԤԼ״̬
    }

}
