using TollCalculator.Domain.TaxRule.Interface;

namespace TollCalculator.Domain.TaxRule.Impl;

public class GothenburgTaxRule : ITollFeeTaxRule
{
    public int Id { get; set; }
    public string City { get; set; }
    public int MaxDailyRate { get; set; }
    public MultiPassageRule MultiPassageRule { get; set; }
    public List<TaxRateTimeSpan> TaxRates { get; set; }
    public List<string> TaxExemptVehicles { get; set; }
    public bool TollFreeWeekends { get; set; }
    public List<DateTime> TollFreeDates { get; set; }
}