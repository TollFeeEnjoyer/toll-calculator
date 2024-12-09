using Microsoft.EntityFrameworkCore;
using TollCalculator.Data.DatabaseContext;
using TollCalculator.Data.Impl;
using TollCalculator.Data.Interface;
using TollCalculator.Domain.TaxRule.Impl;
using TollCalculator.Domain.TaxRule.Interface;
using TollCalculator.Service.Impl;
using TollCalculator.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddScoped<ICustomDatabaseClient, CustomDatabaseClient>();
builder.Services.AddScoped<ITollFeeService, TollFeeService>();

// Because at least on my PC there were some issues with SSL-certificate, I had to disable it
// Also had to disable the SSL certificate check in my insomnia client
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
    serverOptions.ListenAnyIP(7109);
    serverOptions.ListenAnyIP(5017, listenOptions => listenOptions.UseHttps());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.TaxRules.AddRange(
            new TollFeeTaxRule
            {
                City = "Gothenburg",
                MultiPassageRule = new MultiPassageRule()
                {
                    FeeWindowDurationInMinutes = 60
                },
                MaxDailyRate = 60,
                TollFreeWeekends = true,
                TaxExemptVehicles = GetTaxExemptVehicles(),
                TaxRates = GetTaxRateTimeSpan(),
                TollFreeDates = GetTollFreeDates()
            }
        );
        context.SaveChanges();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

List<TaxRateTimeSpan> GetTaxRateTimeSpan()
{
    return new List<TaxRateTimeSpan>
    {
        new() { Rate = 8, StartTime = new TimeOnly(6, 0), EndTime = new TimeOnly(6, 29) },
        new() { Rate = 13, StartTime = new TimeOnly(6, 30), EndTime = new TimeOnly(6, 59) },
        new() { Rate = 18, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(7, 59) },
        new() { Rate = 13, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 29) },
        new() { Rate = 8, StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 59) },
        new() { Rate = 13, StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 29) },
        new() { Rate = 18, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 59) },
        new() { Rate = 13, StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 59) },
        new() { Rate = 8, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(18, 29) },
    };
}

List<TaxExemptVehicle> GetTaxExemptVehicles()
{
    var taxExemptVehicles = new List<TaxExemptVehicle>();
    foreach (var vehicleType in new[] { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Military" })
    {
        taxExemptVehicles.Add(new() { VehicleType = vehicleType });
    }

    return taxExemptVehicles;
}

List<TollFreeDate> GetTollFreeDates()
{
    var tollFreeDates = new List<TollFreeDate>
    {
        new() { Date = new DateOnly(2024, 1, 1) },
        new() { Date = new DateOnly(2024, 3, 28) },
        new() { Date = new DateOnly(2024, 3, 29) },
        new() { Date = new DateOnly(2024, 4, 1) },
        new() { Date = new DateOnly(2024, 4, 30) },
        new() { Date = new DateOnly(2024, 5, 1) },
        new() { Date = new DateOnly(2024, 5, 8) },
        new() { Date = new DateOnly(2024, 5, 9) },
        new() { Date = new DateOnly(2024, 6, 5) },
        new() { Date = new DateOnly(2024, 6, 6) },
        new() { Date = new DateOnly(2024, 6, 21) },
        new() { Date = new DateOnly(2024, 11, 1) },
        new() { Date = new DateOnly(2024, 12, 24) },
        new() { Date = new DateOnly(2024, 12, 25) },
        new() { Date = new DateOnly(2024, 12, 26) },
        new() { Date = new DateOnly(2024, 12, 31) },
    };
    for (var day = 1; day <= 31; day++)
    {
        tollFreeDates.Add(new TollFreeDate { Date = new DateOnly(2024, 7, day) });
    }

    return tollFreeDates;
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();