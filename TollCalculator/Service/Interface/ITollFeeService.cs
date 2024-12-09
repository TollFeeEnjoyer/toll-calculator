using TollCalculator.Domain;

namespace TollCalculator.Service.Interface;

public interface ITollFeeService
{
    Task<int> GetDailyTollFee(Vehicle vehicle, List<DateTime> date, string city);
}