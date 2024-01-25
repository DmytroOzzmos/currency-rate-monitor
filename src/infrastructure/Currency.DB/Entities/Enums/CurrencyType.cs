using System.Text.Json.Serialization;

namespace Currency.DB;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CurrencyType
{
    USD,
    EUR,
    PLN,
    UAH,
}