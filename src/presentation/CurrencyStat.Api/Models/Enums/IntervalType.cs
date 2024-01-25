using System.Text.Json.Serialization;

namespace CurrencyStat.Api;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IntervalType
{
    OneMinute,
    FiveMinutes,
    OneHour,
    OneDay,
    OneWeek,
}