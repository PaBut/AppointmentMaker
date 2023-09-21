namespace AppointmentMaker.Domain.Extensions;

public static class DateTimeExtensions
{
    public static bool IsDate(this DateTime dateTime, DateOnly date) =>
        dateTime.Year == date.Year &&
        dateTime.Month == date.Month &&
        dateTime.Day == date.Day;
}
