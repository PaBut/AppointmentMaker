using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.Extensions;
using AppointmentMaker.Domain.Models;

namespace AppointmentMaker.Domain.Configuration;

public class ScheduleConfiguration
{
    public int DayLength { get; set; }
    public int VisitLengthInMinutes { get; set; }

    private const int hoursInDay = 24;
    private const int minutesInHour = 60;
    private const int bitsInByte = 8;
    private const int daysInWeek = 7;
    private const int monthsInYear = 12;


    public int SlotsInDay => VisitsInDay / bitsInByte;

    public int SlotsInWeek => VisitsInWeek / bitsInByte;

    public int ScheduleSlotsByteLength => DayLength * hoursInDay * minutesInHour /
        (bitsInByte * VisitLengthInMinutes);

    public int VisitsInWeek => daysInWeek * hoursInDay * minutesInHour /
        VisitLengthInMinutes;

    public int VisitsInDay => hoursInDay * minutesInHour /
        VisitLengthInMinutes;

    public int GetDayTimeIndex(DateTime time)
    {
        return GetVisitIndexByTime(time) / bitsInByte;
    }

    public int GetVisitIndexByTime(DateTime time)
    {
        int minutes = time.Hour * minutesInHour + time.Minute;
        return minutes / VisitLengthInMinutes;
    }

    public int GetWeekDayIndex(DayOfWeek dayOfWeek)
    {
        const int sundayIndex = 6;
        int dayOffset = dayOfWeek - DayOfWeek.Monday;
        // Sunday index in DayOfWeek is 0, but we need it to be 7th
        // that's why we're changing -1 to 6
        dayOffset = dayOffset == -1 ? sundayIndex : dayOffset;
        return dayOffset * SlotsInDay;
    }

    public int MonthSlotCount(int month, int year)
    {
        if (DateTime.UtcNow.Month == month && DateTime.UtcNow.Year == year)
            return (DateTime.DaysInMonth(year, month) - DateTime.UtcNow.Day + 1)
                * SlotsInDay;

        return DateTime.DaysInMonth(year, month) * SlotsInDay;
    }

    public int GetMonthIndex(int month, int year)
    {
        DateTime timeNow = DateTime.UtcNow;

        if (timeNow.Year == year && timeNow.Month == month)
        {
            return 0;
        }

        int yearOffset = year - timeNow.Year;
        int monthOffset = yearOffset * monthsInYear + month - timeNow.Month - 1;

        int startIndex = MonthSlotCount(timeNow.Month, timeNow.Year);

        for (int i = 1; i <= monthOffset; i++)
        {
            int monthIndex = (timeNow.Month + i) % (monthsInYear + 1) + (timeNow.Month + i) / (monthsInYear + 1);
            int yearIndex = timeNow.Year + (timeNow.Month + i) / (monthsInYear + 1);
            startIndex += MonthSlotCount(monthIndex, yearIndex);
        }

        return startIndex;
    }

    public TimeSpan GetTimeByIndex(int index)
    {
        index = index % SlotsInDay;
        int minutes = index * bitsInByte * VisitLengthInMinutes;
        return TimeSpan.FromMinutes(minutes);
    }

    public byte[] GetByteTemplateFromTimeIntervals(TimeInterval[] timeIntervals) =>
        GetBoolTemplateFromTimeIntervals(timeIntervals).ToByteArray();

    public bool[] GetBoolTemplateFromTimeIntervals(TimeInterval[] timeIntervals)
    {
        bool[] boolTemplate = new bool[VisitsInWeek];

        foreach (TimeInterval interval in timeIntervals)
        {
            int weekOffset = interval.Start.DayOfWeek.WeekOffset();

            int startIndex = weekOffset * VisitsInDay + GetVisitIndexByTime(interval.Start.Time.ToDateTime());
            int endIndex = weekOffset * VisitsInDay + GetVisitIndexByTime(interval.End.Time.ToDateTime());

            for (int i = startIndex; i < endIndex; i++)
            {
                boolTemplate[i] = true;
            }
        }

        return boolTemplate;
    }
}
