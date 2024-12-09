using TollCalculator.Domain.TaxRule.Impl;

namespace TollCalculator.Domain.TaxRule.Interface;

public interface ITollFeeTaxRule
{
    int Id { get; set; }
    string City { get; set; }
    int MaxDailyRate { get; set; }
    MultiPassageRule MultiPassageRule { get; set; }
    List<TaxRateTimeSpan> TaxRates { get; }
    List<TaxExemptVehicle> TaxExemptVehicles { get; }
    bool TollFreeWeekends { get; set; }
    List<TollFreeDate> TollFreeDates { get; }
}

public class TaxRateTimeSpan
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Rate { get; set; }
    public List<TollFeeTaxRule> TollFeeTaxRule { get; }
}

public class MultiPassageRule
{
    public int Id { get; set; }
    public int FeeWindowDurationInMinutes { get; set; }
}

public class TaxExemptVehicle
{
    public int Id { get; set; }
    public string VehicleType { get; set; }
    public List<TollFeeTaxRule> TollFeeTaxRule { get; set; }
}

public class TollFreeDate
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public List<TollFeeTaxRule> TollFeeTaxRule { get; }
}