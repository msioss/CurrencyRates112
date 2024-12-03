using Currency.Domain.Options;
using Currency.Services.Interfaces;
using Currency.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<PrivatBankApiOptions>(builder.Configuration.GetSection("PrivatBankApiOptions"));
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddSingleton<ICurrencyFileService, CurrencyFileService>();
builder.Services.AddHttpClient<CurrencyRateService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/rates", async (CurrencyRateService currencyRateService) =>
{
    try
    {
        var rates = await currencyRateService.GetAllRatesAsync();
        
        return Results.Ok(rates);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapGet("/rates/{currencyCode}", async (string currencyCode, [FromServices]CurrencyRateService currencyRateService) =>
{
    try
    {
        var rates = await currencyRateService.GetAllRatesAsync();
        var rate = rates.FirstOrDefault(x=>x.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        
        if(rate == null)
            return Results.NotFound($"Rate for {currencyCode} not found");
        
        return Results.Ok(rate);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapPost("/rates/save", async ([FromServices]CurrencyRateService currencyRateService, [FromServices]CurrencyFileService currencyFileService, IWebHostEnvironment env) =>
{
    try
    {
        var rates = await currencyRateService.GetAllRatesAsync();
        await currencyFileService.SaveToFileAsync(rates, Path.Combine(env.WebRootPath, "rates.json"));
        
        return Results.Ok("Rates saved");
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapGet("/rates/load", async ([FromServices]CurrencyRateService currencyRateService, [FromServices]CurrencyFileService currencyFileService, IWebHostEnvironment env) =>
{
    try
    {
        var rates = await currencyFileService.LoadFromFileAsync(Path.Combine(env.WebRootPath, "rates.json"));
        
        return Results.Ok(rates);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.Run();
