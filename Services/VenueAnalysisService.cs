using Microsoft.EntityFrameworkCore;
using VenueBookingSystem.Data;
using VenueBookingSystem.Dto;
using VenueBookingSystem.Models;
using System;
using System.Linq;

namespace VenueBookingSystem.Services
{
    public class VenueAnalysisService : IVenueAnalysisService
    {
        private readonly ApplicationDbContext _context;

        public VenueAnalysisService(ApplicationDbContext context)
        {
            _context = context;
        }

        public VenueAnalysisDto GetVenueAnalysisData(string venueType, DateTime? currentTime = null)
        {
            if (currentTime == null)
            {
                currentTime = DateTime.Now;
            }

            // 查询每日预约人次和营收
            var dailyDataQuery = from r in _context.Reservations
                                 join ur in _context.UserReservations on r.ReservationId equals ur.ReservationId
                                 join v in _context.Venues on r.VenueId equals v.VenueId
                                 where v.Type == venueType
                                 group new { ur.NumOfPeople, r.PaymentAmount } by new { v.VenueId, Date = r.ReservationTime.Date } into g
                                 select new
                                 {
                                     VenueId = g.Key.VenueId,
                                     Date = g.Key.Date,
                                     Population = g.Sum(x => x.NumOfPeople),
                                     Revenue = g.Sum(x => x.PaymentAmount)
                                 };

            var dailyData = dailyDataQuery.ToList();

            if (!dailyData.Any())
            {
                return new VenueAnalysisDto
                {
                    ReserveDescription = new ReserveDescriptionDto
                    {
                        Sum = new TimePeriodDataDto(),
                        Max = new TimePeriodDataDto(),
                        Avg = new TimePeriodDataDto()
                    },
                    RevenueDescription = new RevenueDescriptionDto
                    {
                        Sum = new TimePeriodDataDto(),
                        Max = new TimePeriodDataDto(),
                        Avg = new TimePeriodDataDto()
                    },
                    ReserveDataList = new ReserveDataListDto(),
                    RevenueDataList = new RevenueDataListDto()
                };
            }

            // 计算周、月、年的统计数据
            var weeklyData = dailyData.Where(d => d.Date >= currentTime.Value.AddDays(-7) && d.Date <= currentTime.Value).ToList();
            var monthlyData = dailyData.Where(d => d.Date >= currentTime.Value.AddMonths(-1) && d.Date <= currentTime.Value).ToList();
            var yearlyData = dailyData.Where(d => d.Date >= currentTime.Value.AddYears(-1) && d.Date <= currentTime.Value).ToList();

            // 计算全年总预约人次和总营收
            var totalPopulationYearly = yearlyData.Sum(d => d.Population);
            var totalRevenueYearly = yearlyData.Sum(d => d.Revenue);

            // 构建数据列表
            var weeklyLabels = weeklyData.Select(d => d.Date.ToString("MM-dd")).ToArray();
            var weeklyPopulationData = weeklyData.Select(d => d.Population).ToArray();
            var weeklyRevenueData = weeklyData.Select(d => d.Revenue).ToArray();

            var monthlyLabels = monthlyData.Select(d => d.Date.ToString("MM-dd")).ToArray();
            var monthlyPopulationData = monthlyData.Select(d => d.Population).ToArray();
            var monthlyRevenueData = monthlyData.Select(d => d.Revenue).ToArray();

            var yearlyLabels = yearlyData.Select(d => d.Date.ToString("yyyy-MM")).Distinct().ToArray();
            var yearlyPopulationData = yearlyData.GroupBy(d => d.Date.ToString("yyyy-MM"))
                                                  .Select(g => g.Sum(d => d.Population)).ToArray();
            var yearlyRevenueData = yearlyData.GroupBy(d => d.Date.ToString("yyyy-MM"))
                                              .Select(g => g.Sum(d => d.Revenue)).ToArray();

            // 构建返回对象
            var result = new VenueAnalysisDto
            {
                ReserveDescription = new ReserveDescriptionDto
                {
                    Sum = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "总预约人次", Value = weeklyData.Sum(d => d.Population) },
                        Monthly = new DataItemDto { Title = "总预约人次", Value = monthlyData.Sum(d => d.Population) },
                        Yearly = new DataItemDto { Title = "总预约人次", Value = totalPopulationYearly }
                    },
                    Max = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日最高预约人次", Value = weeklyData.Any() ? weeklyData.Max(d => d.Population) : 0 },
                        Monthly = new DataItemDto { Title = "日最高预约人次", Value = monthlyData.Any() ? monthlyData.Max(d => d.Population) : 0 },
                        Yearly = new DataItemDto { Title = "月最高预约人次", Value = yearlyData.Any() ? yearlyData.Max(d => d.Population) : 0 }
                    },
                    Avg = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日平均预约人次", Value = weeklyData.Any() ? (decimal)weeklyData.Average(d => d.Population) : 0 },
                        Monthly = new DataItemDto { Title = "日平均预约人次", Value = monthlyData.Any() ? (decimal)monthlyData.Average(d => d.Population) : 0 },
                        Yearly = new DataItemDto { Title = "月平均预约人次", Value = Math.Round(totalPopulationYearly / 12.0m, 2) }
                    }
                },
                RevenueDescription = new RevenueDescriptionDto
                {
                    Sum = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "总营收", Value = weeklyData.Sum(d => d.Revenue) },
                        Monthly = new DataItemDto { Title = "总营收", Value = monthlyData.Sum(d => d.Revenue) },
                        Yearly = new DataItemDto { Title = "总营收", Value = totalRevenueYearly }
                    },
                    Max = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日最高营收", Value = weeklyData.Any() ? weeklyData.Max(d => d.Revenue) : 0 },
                        Monthly = new DataItemDto { Title = "日最高营收", Value = monthlyData.Any() ? monthlyData.Max(d => d.Revenue) : 0 },
                        Yearly = new DataItemDto { Title = "月最高营收", Value = yearlyData.Any() ? yearlyData.Max(d => d.Revenue) : 0 }
                    },
                    Avg = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日平均营收", Value = weeklyData.Any() ? weeklyData.Average(d => d.Revenue) : 0 },
                        Monthly = new DataItemDto { Title = "日平均营收", Value = monthlyData.Any() ? monthlyData.Average(d => d.Revenue) : 0 },
                        Yearly = new DataItemDto { Title = "月平均营收", Value = Math.Round(totalRevenueYearly / 12.0m, 2) }
                    }
                },
                ReserveDataList = new ReserveDataListDto
                {
                    Weekly = new DataListDto { Labels = weeklyLabels, Data = weeklyPopulationData },
                    Monthly = new DataListDto { Labels = monthlyLabels, Data = monthlyPopulationData },
                    Yearly = new DataListDto { Labels = yearlyLabels, Data = yearlyPopulationData }
                },
                RevenueDataList = new RevenueDataListDto
                {
                    Weekly = new DataListDto 
                    { 
                        Labels = weeklyLabels, 
                        Data = weeklyRevenueData.Select(d => (int)d).ToArray() 
                    },
                    Monthly = new DataListDto 
                    { 
                        Labels = monthlyLabels, 
                        Data = monthlyRevenueData.Select(d => (int)d).ToArray() 
                    },
                    Yearly = new DataListDto 
                    { 
                        Labels = yearlyLabels, 
                        Data = yearlyRevenueData.Select(d => (int)d).ToArray() 
                    }
                }
            };

            return result;
        }

        public VenueAnalysisDto GetSingleVenueAnalysisData(string venueId, DateTime? currentTime = null)
        {
            if (currentTime == null)
            {
                currentTime = DateTime.Now;
            }

            // 查询每日预约人次和营收
            var dailyDataQuery = from r in _context.Reservations
                                 join ur in _context.UserReservations on r.ReservationId equals ur.ReservationId
                                 join v in _context.Venues on r.VenueId equals v.VenueId
                                 where v.VenueId == venueId
                                 group new { ur.NumOfPeople, r.PaymentAmount } by new { v.VenueId, Date = r.ReservationTime.Date } into g
                                 select new
                                 {
                                     VenueId = g.Key.VenueId,
                                     Date = g.Key.Date,
                                     Population = g.Sum(x => x.NumOfPeople),
                                     Revenue = g.Sum(x => x.PaymentAmount)
                                 };

            var dailyData = dailyDataQuery.ToList();

            if (!dailyData.Any())
            {
                return new VenueAnalysisDto
                {
                    ReserveDescription = new ReserveDescriptionDto
                    {
                        Sum = new TimePeriodDataDto(),
                        Max = new TimePeriodDataDto(),
                        Avg = new TimePeriodDataDto()
                    },
                    RevenueDescription = new RevenueDescriptionDto
                    {
                        Sum = new TimePeriodDataDto(),
                        Max = new TimePeriodDataDto(),
                        Avg = new TimePeriodDataDto()
                    },
                    ReserveDataList = new ReserveDataListDto(),
                    RevenueDataList = new RevenueDataListDto()
                };
            }

            // 计算周、月、年的统计数据
            var weeklyData = dailyData.Where(d => d.Date >= currentTime.Value.AddDays(-7) && d.Date <= currentTime.Value).ToList();
            var monthlyData = dailyData.Where(d => d.Date >= currentTime.Value.AddMonths(-1) && d.Date <= currentTime.Value).ToList();
            var yearlyData = dailyData.Where(d => d.Date >= currentTime.Value.AddYears(-1) && d.Date <= currentTime.Value).ToList();

            // 计算全年总预约人次和总营收
            var totalPopulationYearly = yearlyData.Sum(d => d.Population);
            var totalRevenueYearly = yearlyData.Sum(d => d.Revenue);

            // 构建数据列表
            var weeklyLabels = weeklyData.Select(d => d.Date.ToString("MM-dd")).ToArray();
            var weeklyPopulationData = weeklyData.Select(d => d.Population).ToArray();
            var weeklyRevenueData = weeklyData.Select(d => d.Revenue).ToArray();

            var monthlyLabels = monthlyData.Select(d => d.Date.ToString("MM-dd")).ToArray();
            var monthlyPopulationData = monthlyData.Select(d => d.Population).ToArray();
            var monthlyRevenueData = monthlyData.Select(d => d.Revenue).ToArray();

            var yearlyLabels = yearlyData.Select(d => d.Date.ToString("yyyy-MM")).Distinct().ToArray();
            var yearlyPopulationData = yearlyData.GroupBy(d => d.Date.ToString("yyyy-MM"))
                                                  .Select(g => g.Sum(d => d.Population)).ToArray();
            var yearlyRevenueData = yearlyData.GroupBy(d => d.Date.ToString("yyyy-MM"))
                                              .Select(g => g.Sum(d => d.Revenue)).ToArray();

            // 构建返回对象
            var result = new VenueAnalysisDto
            {
                ReserveDescription = new ReserveDescriptionDto
                {
                    Sum = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "总预约人次", Value = weeklyData.Sum(d => d.Population) },
                        Monthly = new DataItemDto { Title = "总预约人次", Value = monthlyData.Sum(d => d.Population) },
                        Yearly = new DataItemDto { Title = "总预约人次", Value = totalPopulationYearly }
                    },
                    Max = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日最高预约人次", Value = weeklyData.Any() ? weeklyData.Max(d => d.Population) : 0 },
                        Monthly = new DataItemDto { Title = "日最高预约人次", Value = monthlyData.Any() ? monthlyData.Max(d => d.Population) : 0 },
                        Yearly = new DataItemDto { Title = "月最高预约人次", Value = yearlyData.Any() ? yearlyData.Max(d => d.Population) : 0 }
                    },
                    Avg = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日平均预约人次", Value = weeklyData.Any() ? (decimal)weeklyData.Average(d => d.Population) : 0 },
                        Monthly = new DataItemDto { Title = "日平均预约人次", Value = monthlyData.Any() ? (decimal)monthlyData.Average(d => d.Population) : 0 },
                        Yearly = new DataItemDto { Title = "月平均预约人次", Value = Math.Round(totalPopulationYearly / 12.0m, 2) }
                    }
                },
                RevenueDescription = new RevenueDescriptionDto
                {
                    Sum = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "总营收", Value = weeklyData.Sum(d => d.Revenue) },
                        Monthly = new DataItemDto { Title = "总营收", Value = monthlyData.Sum(d => d.Revenue) },
                        Yearly = new DataItemDto { Title = "总营收", Value = totalRevenueYearly }
                    },
                    Max = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日最高营收", Value = weeklyData.Any() ? weeklyData.Max(d => d.Revenue) : 0 },
                        Monthly = new DataItemDto { Title = "日最高营收", Value = monthlyData.Any() ? monthlyData.Max(d => d.Revenue) : 0 },
                        Yearly = new DataItemDto { Title = "月最高营收", Value = yearlyData.Any() ? yearlyData.Max(d => d.Revenue) : 0 }
                    },
                    Avg = new TimePeriodDataDto
                    {
                        Weekly = new DataItemDto { Title = "日平均营收", Value = weeklyData.Any() ? weeklyData.Average(d => d.Revenue) : 0 },
                        Monthly = new DataItemDto { Title = "日平均营收", Value = monthlyData.Any() ? monthlyData.Average(d => d.Revenue) : 0 },
                        Yearly = new DataItemDto { Title = "月平均营收", Value = Math.Round(totalRevenueYearly / 12.0m, 2) }
                    }
                },
                ReserveDataList = new ReserveDataListDto
                {
                    Weekly = new DataListDto { Labels = weeklyLabels, Data = weeklyPopulationData },
                    Monthly = new DataListDto { Labels = monthlyLabels, Data = monthlyPopulationData },
                    Yearly = new DataListDto { Labels = yearlyLabels, Data = yearlyPopulationData }
                },
                RevenueDataList = new RevenueDataListDto
                {
                    Weekly = new DataListDto 
                    { 
                        Labels = weeklyLabels, 
                        Data = weeklyRevenueData.Select(d => (int)d).ToArray() 
                    },
                    Monthly = new DataListDto 
                    { 
                        Labels = monthlyLabels, 
                        Data = monthlyRevenueData.Select(d => (int)d).ToArray() 
                    },
                    Yearly = new DataListDto 
                    { 
                        Labels = yearlyLabels, 
                        Data = yearlyRevenueData.Select(d => (int)d).ToArray() 
                    }
                }

            };

            return result;
        }
    }
}
