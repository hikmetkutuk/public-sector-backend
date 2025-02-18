namespace Domain.Helpers;

public static class DateTimeHelper
{
    private static readonly TimeZoneInfo TurkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");

    public static DateTimeOffset GetTurkeyTime()
    {
        var utcNow = DateTime.UtcNow;
        return TimeZoneInfo.ConvertTime(new DateTimeOffset(utcNow), TurkeyTimeZone);
    }
}