using TollCalculator.Domain.TaxRule.Interface;

namespace TollCalculator.Domain.TaxRule.Impl;

public class TollFeeTaxRule : ITollFeeTaxRule
{
    public int Id { get; set; }
    public string City { get; set; }
    public int MaxDailyRate { get; set; }
    public MultiPassageRule MultiPassageRule { get; set; }
    public int MultiPassageRuleId { get; set; }
    public List<TaxRateTimeSpan> TaxRates { get; set; } = new();
    public List<TaxExemptVehicle> TaxExemptVehicles { get; set; } = new();
    public bool TollFreeWeekends { get; set; }
    public List<TollFreeDate> TollFreeDates { get; set; } = new();
}