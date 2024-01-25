using ProtoBuf;

namespace CurrencyInfo.WS;

[ProtoContract]
public class CurrencyRate
{
    [ProtoMember(1)]
    public string Currency { get; set; }
    [ProtoMember(2)]
    public double Price { get; set; }
    [ProtoMember(3)]
    public DateTime Timestamp { get; set; }
}