namespace AppointmentMaker.Domain.Extentions;

public static class TimeOnlyExtentions
{
    public static DateTime ToDateTime(this TimeOnly time)
    {
        var referenceDate = new DateTime(1, 1, 1);
        return referenceDate + time.ToTimeSpan();
    }
}
