namespace AppointmentMaker.Domain.Models;

public class WeekTime : IComparable<WeekTime>
{
    public TimeOnly Time { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    public int CompareTo(WeekTime? other)
    {
        if (DayOfWeek > other.DayOfWeek
            || (DayOfWeek == other.DayOfWeek && Time > other.Time))
            return 1;
        else if (DayOfWeek == other.DayOfWeek && Time == other.Time)
            return 0;
        else
            return -1;
    }

    public static bool operator >(WeekTime left, WeekTime right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(WeekTime left, WeekTime right)
    {
        return left.CompareTo(right) < 0;
    }
}
