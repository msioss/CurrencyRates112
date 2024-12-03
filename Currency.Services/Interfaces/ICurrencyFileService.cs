using Currency.Domain.DtoModels;

namespace Currency.Services.Interfaces;

public interface ICurrencyFileService
{
    Task SaveToFileAsync(List<CurrencyRateDto> rates, string path);
    Task<List<CurrencyRateDto>> LoadFromFileAsync(string path);
}