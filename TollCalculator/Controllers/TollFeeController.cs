using Microsoft.AspNetCore.Mvc;
using TollCalculator.Domain;
using TollCalculator.Service.Interface;

namespace TollCalculator.Controllers;

[ApiController]
[Route("tax")]
public class TollFeeController : ControllerBase
{
    private readonly ILogger<TollFeeController> _logger;
    private readonly ITollFeeService _tollFeeService;

    public TollFeeController(ILogger<TollFeeController> logger, ITollFeeService tollFeeService)
    {
        _logger = logger;
        _tollFeeService = tollFeeService;
    }

    [HttpPost("/tollFee/{city}")]
    public async Task<IActionResult> CalculateDailyTollFee([FromBody] DailyTollFeeRequest request, string city)
    {
        if (string.IsNullOrEmpty(city))
        {
            return new BadRequestObjectResult(new { message = "No city provided" });
        }

        if (request.Dates.Count == 0)
        {
            return new BadRequestObjectResult(new { message = "No dates provided" });
        }

        request.Dates.Sort();
        if (request.Dates.First().Date != request.Dates.Last().Date)
        {
            return new BadRequestObjectResult(new { message = "The dates must be the same day" });
        }

        bool isValidVehicleType = Enum.TryParse<VehicleTypes>(request.Vehicle.Type, out _);
        if (!isValidVehicleType)
        {
            return new BadRequestObjectResult(new { message = "No vehicle provided" });
        }

        return new OkObjectResult(
            new { dailyTollFee = await _tollFeeService.GetDailyTollFee(request.Vehicle, request.Dates, city) });
    }
}

public class DailyTollFeeRequest
{
    public Vehicle Vehicle { get; set; }
    public List<DateTime> Dates { get; set; }
}