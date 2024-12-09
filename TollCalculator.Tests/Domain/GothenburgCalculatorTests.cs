using System;
using System.Collections.Generic;
using TollCalculator.Domain;
using TollCalculator.Domain.Calculator.Impl;
using TollCalculator.Domain.TaxRule.Impl;
using TollCalculator.Domain.TaxRule.Interface;
using Xunit;

namespace TollCalculator.Tests.Domain;

public class GothenburgCalculatorTests
{
    [Fact]
    public void GetDailyTollFee()
    {
        var taxRule = GetTollFeeTaxRule();
        var calculator = new GothenburgTollFeeCalculator(taxRule);
        var tollPassages = GetDateTimes();
        var vehicle = new Vehicle() { Type = "Car" };

        var result = calculator.GetDailyTollFee(tollPassages, vehicle);

        Assert.Equal(44, result);
    }

    [Fact]
    public void GetDailyTollFee_TaxExempt()
    {
        var taxRule = GetTollFeeTaxRule();
        var calculator = new GothenburgTollFeeCalculator(taxRule);
        var tollPassages = GetDateTimes();
        var vehicle = new Vehicle() { Type = "Diplomat" };

        var result = calculator.GetDailyTollFee(tollPassages, vehicle);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetDailyTollFee_Weekend()
    {
        var taxRule = GetTollFeeTaxRule();
        var calculator = new GothenburgTollFeeCalculator(taxRule);
        var tollPassages = new List<DateTime>() { DateTime.Parse("2024-12-08T07:15:00") };
        var vehicle = new Vehicle() { Type = "Car" };

        var result = calculator.GetDailyTollFee(tollPassages, vehicle);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetDailyTollFee_TollFreeDate()
    {
        var taxRule = GetTollFeeTaxRule();
        var calculator = new GothenburgTollFeeCalculator(taxRule);
        var tollPassages = new List<DateTime>() { DateTime.Parse("2024-01-01T07:15:00") };
        var vehicle = new Vehicle() { Type = "Car" };

        var result = calculator.GetDailyTollFee(tollPassages, vehicle);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetDailyTollFee_DailyMax()
    {
        var taxRule = GetTollFeeTaxRule();
        var calculator = new GothenburgTollFeeCalculator(taxRule);
        var tollPassages = GetDateTimesDailyMax();
        var vehicle = new Vehicle() { Type = "Car" };

        var result = calculator.GetDailyTollFee(tollPassages, vehicle);

        Assert.Equal(60, result);
    }

    private static List<DateTime> GetDateTimes()
    {
        return new List<DateTime>()
        {
            DateTime.Parse("2024-05-22T07:15:00"),
            DateTime.Parse("2024-05-22T08:10:30"),
            DateTime.Parse("2024-05-22T09:14:25"),
            DateTime.Parse("2024-05-22T10:05:30"),
            DateTime.Parse("2024-05-22T16:15:48"),
        };
    }

    private static TollFeeTaxRule GetTollFeeTaxRule()
    {
        return new TollFeeTaxRule
        {
            City = "Gothenburg",
            MultiPassageRule = new MultiPassageRule()
            {
                FeeWindowDurationInMinutes = 60
            },
            MaxDailyRate = 60,
            TollFreeWeekends = true,
            TaxExemptVehicles = GetTaxExemptVehicles(),
            TaxRates = GetTaxRateTimeSpan(),
            TollFreeDates = GetTollFreeDates()
        };
    }

    private static List<DateTime> GetDateTimesDailyMax()
    {
        return new List<DateTime>()
        {
            DateTime.Parse("2024-05-22T06:01:00"),
            DateTime.Parse("2024-05-22T07:02:00"),
            DateTime.Parse("2024-05-22T08:03:30"),
            DateTime.Parse("2024-05-22T09:04:25"),
            DateTime.Parse("2024-05-22T10:05:30"),
            DateTime.Parse("2024-05-22T11:06:30"),
            DateTime.Parse("2024-05-22T12:07:30"),
            DateTime.Parse("2024-05-22T13:08:30"),
            DateTime.Parse("2024-05-22T14:09:30"),
            DateTime.Parse("2024-05-22T15:10:30"),
            DateTime.Parse("2024-05-22T16:11:48"),
            DateTime.Parse("2024-05-22T17:12:48"),
            DateTime.Parse("2024-05-22T18:13:48"),
        };
    }


    private static List<TaxRateTimeSpan> GetTaxRateTimeSpan()
    {
        return new List<TaxRateTimeSpan>
        {
            new() { Rate = 8, StartTime = new TimeOnly(6, 0), EndTime = new TimeOnly(6, 29) },
            new() { Rate = 13, StartTime = new TimeOnly(6, 30), EndTime = new TimeOnly(6, 59) },
            new() { Rate = 18, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(7, 59) },
            new() { Rate = 13, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 29) },
            new() { Rate = 8, StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 59) },
            new() { Rate = 13, StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 29) },
            new() { Rate = 18, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 59) },
            new() { Rate = 13, StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 59) },
            new() { Rate = 8, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(18, 29) },
        };
    }

    private static List<TaxExemptVehicle> GetTaxExemptVehicles()
    {
        var taxExemptVehicles = new List<TaxExemptVehicle>();
        foreach (var vehicleType in new[] { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Military" })
        {
            taxExemptVehicles.Add(new() { VehicleType = vehicleType });
        }

        return taxExemptVehicles;
    }

    private static List<TollFreeDate> GetTollFreeDates()
    {
        var tollFreeDates = new List<TollFreeDate>
        {
            new() { Date = new DateOnly(2024, 1, 1) },
            new() { Date = new DateOnly(2024, 3, 28) },
            new() { Date = new DateOnly(2024, 3, 29) },
            new() { Date = new DateOnly(2024, 4, 1) },
            new() { Date = new DateOnly(2024, 4, 30) },
            new() { Date = new DateOnly(2024, 5, 1) },
            new() { Date = new DateOnly(2024, 5, 8) },
            new() { Date = new DateOnly(2024, 5, 9) },
            new() { Date = new DateOnly(2024, 6, 5) },
            new() { Date = new DateOnly(2024, 6, 6) },
            new() { Date = new DateOnly(2024, 6, 21) },
            new() { Date = new DateOnly(2024, 11, 1) },
            new() { Date = new DateOnly(2024, 12, 24) },
            new() { Date = new DateOnly(2024, 12, 25) },
            new() { Date = new DateOnly(2024, 12, 26) },
            new() { Date = new DateOnly(2024, 12, 31) },
        };
        for (var day = 1; day <= 31; day++)
        {
            tollFreeDates.Add(new TollFreeDate { Date = new DateOnly(2024, 7, day) });
        }

        return tollFreeDates;
    }
}