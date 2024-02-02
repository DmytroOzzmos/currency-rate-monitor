using System.Text.Json.Serialization;

namespace Currency.DB;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IntervalType
{
    OneMinute,
    FiveMinutes,
    OneHour,
    OneDay,
    OneWeek,
}