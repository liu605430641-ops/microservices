using Quartz;

namespace Zhaoxi.MSACommerce.SecKillSyncWorker.Jobs;

public static class SecKillSyncJobSchedule
{
    private static readonly TriggerKey Key = new(nameof(SecKillSyncJobSchedule), nameof(SecKillSyncWorker));

    public static void CreateSecKillSyncJobSchedule(
        this IScheduler scheduler, int intervalTime = 20)
    {
        var jobDetail = JobBuilder.Create<SecKillSyncJob>()
            .WithIdentity(SecKillSyncJob.Key)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(Key)
            .ForJob(SecKillSyncJob.Key)
            .WithSimpleSchedule(builder => builder
                .RepeatForever()
                .WithInterval(TimeSpan.FromSeconds(intervalTime)))
            .Build();

        scheduler.ScheduleJob(jobDetail, trigger);
    }
}