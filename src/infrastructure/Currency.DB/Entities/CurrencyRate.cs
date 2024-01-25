namespace Currency.DB;

public class CurrencyRate
{
    public Guid Id { get; set; }
    public CurrencyType FromCurrency { get; set; }
    public CurrencyType ToCurrency { get; set; }
    public double Price { get; set; }
    public DateTime Timestamp { get; set; }
}