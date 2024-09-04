namespace VenueBookingSystem.Dto
{
    public class VenueAnalysisDto
    {
        public ReserveDescriptionDto ReserveDescription { get; set; }
        public RevenueDescriptionDto RevenueDescription { get; set; }
        public ReserveDataListDto ReserveDataList { get; set; }
        public RevenueDataListDto RevenueDataList { get; set; }
    }

    public class ReserveDescriptionDto
    {
        public TimePeriodDataDto Sum { get; set; }
        public TimePeriodDataDto Max { get; set; }
        public TimePeriodDataDto Avg { get; set; }
    }

    public class RevenueDescriptionDto
    {
        public TimePeriodDataDto Sum { get; set; }
        public TimePeriodDataDto Max { get; set; }
        public TimePeriodDataDto Avg { get; set; }
    }

    public class TimePeriodDataDto
    {
        public DataItemDto Weekly { get; set; }
        public DataItemDto Monthly { get; set; }
        public DataItemDto Yearly { get; set; }
    }

    public class DataItemDto
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
    }

    public class ReserveDataListDto
    {
        public DataListDto Weekly { get; set; }
        public DataListDto Monthly { get; set; }
        public DataListDto Yearly { get; set; }
    }

    public class RevenueDataListDto
    {
        public DataListDto Weekly { get; set; }
        public DataListDto Monthly { get; set; }
        public DataListDto Yearly { get; set; }
    }

    public class DataListDto
    {
        public string[] Labels { get; set; }
        public int[] Data { get; set; }
    }
}
