namespace AppointmentMaker.Domain.Models;

public class TimeInterval
{
    public WeekTime Start { get; set; }
    public WeekTime End { get; set; }

    public bool IsTimeBetween(WeekTime time) =>
        time > Start && time < End;
}
