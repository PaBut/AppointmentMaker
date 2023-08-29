namespace AppointmentMaker.Domain.Extentions;

public static class DayOfWeekExtentions
{
    public static int WeekOffset(this DayOfWeek dayOfWeek)
    {
        if(dayOfWeek == DayOfWeek.Sunday)
        {
            return 6;
        }
        //Converting index to Monday first week type
        return (int)dayOfWeek - 1;
    }
}
