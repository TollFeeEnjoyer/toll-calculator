using Microsoft.AspNetCore.Mvc;
using TollCalculator.Domain;
using TollCalculator.Service.Interface;

namespace TollCalculator.Controllers;

[ApiController]
[Route("[controller]")]
public class TollFeeController : ControllerBase
{
    private readonly ILogger<TollFeeController> _logger;
    private readonly ITollFeeService _tollFeeService;

    public TollFeeController(ILogger<TollFeeController> logger, ITollFeeService tollFeeService)
    {
        _logger = logger;
        _tollFeeService = tollFeeService;
    }

    [HttpPost(Name = "CalculateDailyTollFee")]
    public IActionResult CalculateDailyTollFee([FromBody] List<DateTime> dateTimes, [FromBody] Vehicle vehicle)
    {
        if (dateTimes.Count == 0)
        {
            return new BadRequestObjectResult(new { message = "No dates provided" });
        }

        dateTimes.Sort();
        if (dateTimes.First().Date != dateTimes.Last().Date)
        {
            return new BadRequestObjectResult(new { message = "The dates must be the same day" });
        }

        bool isValidVehicleType = Enum.TryParse<VehicleTypes>(vehicle.Type, out var _);
        if (!isValidVehicleType)
        {
            return new BadRequestObjectResult(new { message = "No vehicle provided" });
        }

        return new OkObjectResult(new { dailyTollFee = _tollFeeService.GetDailyTollFee(vehicle, dateTimes) });
    }
}

public class DailyTollFeeResponse
{
    public int TollFee { get; set; }
}

public class DailyTollFeeRequest
{
    public Vehicle Vehicle { get; set; }
    public List<DateTime> Dates { get; set; }
}