using TollCalculator.Domain.Calculator.Interface;
using TollCalculator.Domain.TaxRule.Impl;

namespace TollCalculator.Domain.Calculator.Impl;

public class GothenburgTollFeeCalculator : ITollFeeCalculator
{
    private readonly GothenburgTaxRule _taxRule;

    public GothenburgTollFeeCalculator(GothenburgTaxRule taxRule)
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
            if ((TimeOnly.FromDateTime(date) - TimeOnly.FromDateTime(intervalStart)).Minutes <
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

            if (_taxRule.MaxDailyRate < totalDailyFee)
            {
                return _taxRule.MaxDailyRate;
            }
        }

        return totalDailyFee;
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
        return _taxRule.TaxExemptVehicles.Contains(vehicle.Type);
    }

    private bool IsTollFreeDate(DateTime date)
    {
        return _taxRule.TollFreeDates.Contains(date);
    }
}