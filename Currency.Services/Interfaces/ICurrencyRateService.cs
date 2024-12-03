using Currency.Domain.DtoModels;

namespace Currency.Services.Interfaces;

public interface ICurrencyRateService
{
    Task<List<CurrencyRateDto>> GetAllRatesAsync();
}