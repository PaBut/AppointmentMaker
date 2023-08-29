using AppointmentMaker.Infrastructure.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AppointmentMaker.Infrastructure.Setups;

internal class ScheduleShiftBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(ScheduleShiftBackgroundJob));

        options.AddJob<ScheduleShiftBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
        .AddTrigger(t => t.ForJob(jobKey)
        .WithDailyTimeIntervalSchedule(schedule => schedule.OnEveryDay()
        .InTimeZone(TimeZoneInfo.Utc)
        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
        .EndingDailyAfterCount(1)));
    }
}
