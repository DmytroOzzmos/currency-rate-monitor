namespace Currency.DB;

public class CurrencyRateByInterval
{
    public double Low { get; set; }
    public double High { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public int Count { get; set; }
    public IntervalType Interval { get; set; }
    public DateTime Timestamp { get; set; }
}