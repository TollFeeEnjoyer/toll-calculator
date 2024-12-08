using TollCalculator.Domain.Calculator.Impl;
using TollCalculator.Domain.Calculator.Interface;
using TollCalculator.Domain.TaxRule.Impl;
using TollCalculator.Domain.TaxRule.Interface;

namespace TollCalculator.Domain.Factory;

public static class TollFeeCalculatorFactory
{
    public static ITollFeeCalculator CreateCalculator(ITollFeeTaxRule taxRule)
    {
        return taxRule switch
        {
            GothenburgTaxRule => new GothenburgTollFeeCalculator(taxRule as GothenburgTaxRule),
            _ => throw new ArgumentException("Unsupported tax rule")
        };
    }
}