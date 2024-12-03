using System.Net.Http.Json;
using Currency.Domain.DtoModels;
using Currency.Domain.Options;
using Currency.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Currency.Services.Services;

public class CurrencyRateService: ICurrencyRateService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<PrivatBankApiOptions> _options;

    public CurrencyRateService(HttpClient httpClient, IOptions<PrivatBankApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<List<CurrencyRateDto>> GetAllRatesAsync()
    {
        var response = await _httpClient.GetAsync(_options.Value.Route);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<CurrencyRateDto>>();
            return data!;
        }
        
        throw new Exception($"Unable to get currency rates");
    }
}