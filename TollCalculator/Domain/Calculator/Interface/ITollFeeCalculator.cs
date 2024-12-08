namespace TollCalculator.Domain.Calculator.Interface;

public interface ITollFeeCalculator
{
    int GetDailyTollFee(List<DateTime> dates, Vehicle vehicle);
    int GetTollFee(DateTime date, Vehicle vehicle);
}