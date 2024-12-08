namespace TollCalculator.Domain.TaxRule.Interface;

public interface ITollFeeTaxRule
{
    int Id { get; set; }
    string City { get; set; }
    int MaxDailyRate { get; set; }
    MultiPassageRule? MultiPassageRule { get; set; }
    List<TaxRateTimeSpan> TaxRates { get; set; }
    List<string> TaxExemptVehicles { get; set; }
    bool TollFreeWeekends { get; set; }
    List<DateTime> TollFreeDates { get; set; }
}

public class TaxRateTimeSpan
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Rate { get; set; }
}

public class MultiPassageRule
{
    public int FeeWindowDurationInMinutes { get; set; }
    public FeeCalculationMethod FeeCalculationMethod { get; set; }
}

public enum FeeCalculationMethod
{
    Maximum,
    Average
}