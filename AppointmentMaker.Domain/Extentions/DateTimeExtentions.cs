namespace AppointmentMaker.Domain.Extentions;

public static class DateTimeExtentions
{
    public static bool IsDate(this DateTime dateTime, DateOnly date) =>
        dateTime.Year == date.Year &&
        dateTime.Month == date.Month &&
        dateTime.Day == date.Day;
}
