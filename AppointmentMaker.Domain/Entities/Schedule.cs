using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities.Common;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Domain.Extentions;

namespace AppointmentMaker.Domain.Entities;

public class Schedule : Entity
{

    private Schedule(byte[] scheduleTemplate,
        byte[] scheduleSlots, string doctorId)
    {
        ScheduleTemplate = scheduleTemplate;
        ScheduleSlots = scheduleSlots;
        DoctorId = doctorId;
    }

    public string DoctorId { get; set; }
    public byte[] ScheduleSlots { get; set; }
    public byte[] ScheduleTemplate { get; set; }

    public Result UpdateTimeSlot(DateTime time, bool occupy, ScheduleConfiguration scheduleConfiguration)
    {
        int dayOffset = (time - DateTime.UtcNow).Days;
        int index = dayOffset * scheduleConfiguration.SlotsInDay + scheduleConfiguration.GetDayTimeIndex(time);
        int offset = (time.TimeOfDay - scheduleConfiguration.GetTimeByIndex(index)).Minutes / scheduleConfiguration.VisitLengthInMinutes;

        if (((ScheduleSlots[index] >> (byte)offset) & 1) == 0 && occupy)
        {
            return Result.Failure(new Error("Appointment.Create", "Time slot is already taken"));
        }

        byte temp = (byte)(Convert.ToByte(occupy) << offset);

        if (occupy)
        {
            ScheduleSlots[index] |= temp;
        }
        else
        {
            ScheduleSlots[index] &= (byte)~temp;
        }
        
        return Result.Success();
    }

    public bool[] GetScheduleSlotsBoolArray()
    {
        return ScheduleSlots.ToBoolArray();
    }

    public bool[] GetScheduleTemplateBoolArray()
    {
        return ScheduleTemplate.ToBoolArray();
    }

    private static byte[] GetFullScheduleFromBoolTemplate(byte[] byteTemplate, ScheduleConfiguration scheduleConfiguration)
    {
        int dayTemplateIndex = scheduleConfiguration.SlotsInDay * DateTime.UtcNow.DayOfWeek.WeekOffset();

        int scheduleSlotsByteLength = scheduleConfiguration.ScheduleSlotsByteLength;

        int weekSlotsByteLength = byteTemplate.Length;

        byte[] scheduleSlots = new byte[scheduleSlotsByteLength];

        for (int i = 0; i < scheduleSlotsByteLength; i++)
        {
            int templateIndex = (i + dayTemplateIndex) % weekSlotsByteLength;

            scheduleSlots[i] = byteTemplate[templateIndex];
        }

        return scheduleSlots;
    }

    public static Schedule Create(byte[] byteTemplate, 
        string doctorId, 
        ScheduleConfiguration scheduleConfiguration)
    {
        byte[] scheduleSlots = GetFullScheduleFromBoolTemplate(byteTemplate, scheduleConfiguration);
        return new Schedule(byteTemplate, scheduleSlots, doctorId);
    }
}
