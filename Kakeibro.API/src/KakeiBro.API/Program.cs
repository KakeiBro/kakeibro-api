using Asp.Versioning.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// API Configurations
builder.Services.AddOpenApi();
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

// Installing Modules
builder.Services.InstallModulesFromAssemblies(
    builder.Configuration,
    Modules.Authentication.AssemblyReference.Assembly);

// Logging
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

ApiVersionSet apiVersionSet = app
    .NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();
RouteGroupBuilder groupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);

// Add Modules Routes
groupBuilder.InstallEndpointsFromAssemblies(
    app.Configuration,
    Modules.Authentication.AssemblyReference.Assembly);

string[] summaries =
[
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
];

groupBuilder.MapGet("/weatherforecast", () =>
    {
        WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

await app.RunAsync();

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
