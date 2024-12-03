using System.Text.Json.Serialization;

namespace Currency.Domain.DtoModels;

public class CurrencyRateDto
{
    public decimal Buy { get; set; }
    public decimal Sale { get; set; }
    [JsonPropertyName("ccy")]
    public string CurrencyCode { get; set; } = null!;
    [JsonPropertyName("base_ccy")]
    public string BaseCurrency { get; set; } = null!;
}