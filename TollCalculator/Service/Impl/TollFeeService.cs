using System.Reflection;
using System.Text.Json.Serialization;
using TollCalculator.Data.Interface;
using TollCalculator.Domain;
using TollCalculator.Domain.Factory;
using TollCalculator.Domain.TaxRule.Impl;
using TollCalculator.Service.Interface;

namespace TollCalculator.Service.Impl;

public class TollFeeService : ITollFeeService
{
    private readonly ICustomDatabaseClient _customDatabaseClient;

    public TollFeeService(ICustomDatabaseClient customDatabaseClient)
    {
        _customDatabaseClient = customDatabaseClient;
    }

    public async Task<int> GetDailyTollFee(Vehicle vehicle, List<DateTime> dates, string city)
    {
        var taxRule = await _customDatabaseClient.ReadOne<TollFeeTaxRule>(
            x => x.City == city,
            x => x.TaxRates,
            x => x.MultiPassageRule,
            x => x.TaxExemptVehicles,
            x => x.TollFreeDates
        );
        if (taxRule == null) throw new Exception("No tax rule found for the city");
        var calculator = TollFeeCalculatorFactory.CreateCalculator(taxRule);
        return calculator.GetDailyTollFee(dates, vehicle);
    }
}