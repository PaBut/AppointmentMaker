namespace AppointmentMaker.Domain.Extensions;

public static class TimeOnlyExtensions
{
    public static DateTime ToDateTime(this TimeOnly time)
    {
        var referenceDate = new DateTime(1, 1, 1);
        return referenceDate + time.ToTimeSpan();
    }
}
