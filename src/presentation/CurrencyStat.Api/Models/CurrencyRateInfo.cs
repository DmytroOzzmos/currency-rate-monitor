using ProtoBuf;

namespace CurrencyStat.Api;

[ProtoContract]
public class CurrencyRateInfo
{
    [ProtoMember(1)]
    public string Currency { get; set; }
    [ProtoMember(2)]
    public double Price { get; set; }
    [ProtoMember(3)]
    public DateTime Timestamp { get; set; }
}