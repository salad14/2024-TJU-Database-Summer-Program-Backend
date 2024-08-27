public class ReservationDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal PaymentAmount { get; set; }  // 支付金额
    public string UserId { get; set; }  // 用户ID
    public string VenueId { get; set; }  // 场地ID
    public string AvailabilityId { get; set; }  // 开放时间段ID
    public string ReservationItem { get; set; }  // 预约项目描述
    public int NumOfPeople { get; set; }  // 预约人数
}
