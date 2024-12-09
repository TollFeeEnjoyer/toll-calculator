using TollCalculator.Domain.Calculator.Interface;
using TollCalculator.Domain.TaxRule.Impl;
using TollCalculator.Domain.TaxRule.Interface;

namespace TollCalculator.Domain.Calculator.Impl;

public class GothenburgTollFeeCalculator : ITollFeeCalculator
{
    private readonly ITollFeeTaxRule _taxRule;

    public GothenburgTollFeeCalculator(ITollFeeTaxRule taxRule)
    {
        _taxRule = taxRule;
    }


    public int GetDailyTollFee(List<DateTime> dates, Vehicle vehicle)
    {
        var intervalStart = dates[0];
        var highestIntervalFee = 0;
        var totalDailyFee = 0;
        foreach (var date in dates)
        {
            if ((TimeOnly.FromDateTime(date) - TimeOnly.FromDateTime(intervalStart)).TotalMinutes <
                _taxRule.MultiPassageRule.FeeWindowDurationInMinutes)
            {
                highestIntervalFee = Math.Max(highestIntervalFee, GetTollFee(date, vehicle));
            }
            else
            {
                totalDailyFee += highestIntervalFee;
                highestIntervalFee = GetTollFee(date, vehicle);
                intervalStart = date;
            }

            if (totalDailyFee > _taxRule.MaxDailyRate)
            {
                return _taxRule.MaxDailyRate;
            }
        }

        return Math.Min(totalDailyFee + highestIntervalFee, _taxRule.MaxDailyRate);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        var dateAsTimeOnly = TimeOnly.FromDateTime(date);
        var foundTaxRate =
            _taxRule.TaxRates.FirstOrDefault(taxRate => dateAsTimeOnly.IsBetween(taxRate.StartTime, taxRate.EndTime));
        return foundTaxRate?.Rate ?? 0;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return _taxRule.TaxExemptVehicles.Exists(x => x.VehicleType == vehicle.Type);
    }

    private bool IsTollFreeDate(DateTime date)
    {
        return _taxRule.TollFreeDates.Exists(x => x.Date == DateOnly.FromDateTime(date.Date)) ||
               _taxRule.TollFreeWeekends &&
               (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
    }
}