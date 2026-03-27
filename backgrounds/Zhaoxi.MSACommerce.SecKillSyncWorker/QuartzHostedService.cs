using Quartz;
using Quartz.Spi;
using Zhaoxi.MSACommerce.SecKillSyncWorker.Jobs;

namespace Zhaoxi.MSACommerce.SecKillSyncWorker;

public class QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory) : IHostedService
{
    private IScheduler Scheduler { get; } = schedulerFactory.GetScheduler().Result;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = jobFactory;
        Scheduler.CreateSecKillSyncJobSchedule();
        await Scheduler.StartDelayed(TimeSpan.FromSeconds(5), cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Scheduler.Shutdown(cancellationToken);
    }
}
