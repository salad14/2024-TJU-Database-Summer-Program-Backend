public class ReservationDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal PaymentAmount { get; set; }  // ֧�����
    public string UserId { get; set; }  // �û�ID
    public string VenueId { get; set; }  // ����ID
    public string AvailabilityId { get; set; }  // ����ʱ���ID
    public string ReservationItem { get; set; }  // ԤԼ��Ŀ����
    public int NumOfPeople { get; set; }  // ԤԼ����
}
