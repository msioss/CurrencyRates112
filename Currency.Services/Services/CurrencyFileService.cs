using System.Text.Json;
using Currency.Domain.DtoModels;
using Currency.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Currency.Services.Services;

public class CurrencyFileService: ICurrencyFileService
{
    public async Task SaveToFileAsync(List<CurrencyRateDto> rates, string path)
    {
        var json = JsonSerializer.Serialize(rates);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<List<CurrencyRateDto>> LoadFromFileAsync(string path)
    {
        if(!File.Exists(path))
            return [];
        
        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<List<CurrencyRateDto>>(json)?? [];

    }
}