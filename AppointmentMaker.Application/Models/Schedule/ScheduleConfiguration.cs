//using AppointmentMaker.Domain.Extentions;

//namespace AppointmentMaker.Application.Models.Schedule;

//public class ScheduleConfiguration
//{
//    public int DayLength { get; set; }
//    public int VisitLengthInMinutes { get; set; }

//    private const int hoursInDay = 24;
//    private const int minutesInHour = 60;
//    private const int bitsInByte = 8;
//    private const int daysInWeek = 7;

//    public int SlotsInDay => VisitsInDay / bitsInByte;

//    public int SlotsInWeek => VisitsInWeek / bitsInByte;

//    public int ScheduleSlotsByteLength => DayLength * hoursInDay * minutesInHour /
//        (bitsInByte * VisitLengthInMinutes);

//    public int VisitsInWeek => daysInWeek * hoursInDay * minutesInHour /
//        VisitLengthInMinutes;

//    public int VisitsInDay => hoursInDay * minutesInHour /
//        VisitLengthInMinutes;

//    public int GetDayTimeIndex(DateTime time)
//    {
//        return GetVisitIndexByTime(time) / bitsInByte;
//    }

//    public int GetVisitIndexByTime(DateTime time)
//    {
//        int minutes = time.Hour * minutesInHour + time.Minute;
//        return minutes / VisitLengthInMinutes;
//    }

//    public int GetWeekDayIndex(DayOfWeek dayOfWeek)
//    {
//        const int sundayIndex = 6;
//        int dayOffset = dayOfWeek - DayOfWeek.Monday;
//        // Sunday index in DayOfWeek is 0, but we need it to be 7th
//        // that's why we're changing -1 to 6
//        dayOffset = dayOffset == -1 ? sundayIndex : dayOffset;
//        return dayOffset * SlotsInDay;
//    }

//    public TimeSpan GetTimeByIndex(int index)
//    {
//        index = index % SlotsInDay;
//        int minutes = index * bitsInByte * VisitLengthInMinutes;
//        return TimeSpan.FromMinutes(minutes);
//    }

//    public byte[] GetByteTemplateFromTimeIntervals(TimeInterval[] timeIntervals)
//    {
//        bool[] boolTemplate = new bool[VisitsInWeek];

//        foreach (TimeInterval interval in timeIntervals)
//        {
//            int startIndex = GetVisitIndexByTime(Convert.ToDateTime(interval.Start.Time));
//            int endIndex = GetVisitIndexByTime(Convert.ToDateTime(interval.End.Time));

//            for (int i = startIndex; i < endIndex; i++)
//            {
//                boolTemplate[i] = true;
//            }
//        }

//        return boolTemplate.ToByteArray();
//    }

//}
