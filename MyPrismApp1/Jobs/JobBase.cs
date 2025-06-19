using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Jobs;

namespace MyPrismApp1.Jobs;

// Note-PM: taken from shiny samples
public abstract class JobBase : NotifyPropertyChanged, IJob
{

    public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
    {
        var fireJob = true;
        this.JobInfo = jobInfo;
 
        if (this.MinimumTime != null && this.LastRunTime != null)
        {
            var timeDiff = DateTimeOffset.UtcNow.Subtract(this.LastRunTime.Value);
            fireJob = timeDiff >= this.MinimumTime;
            Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Time Difference: {timeDiff} - Firing JobBase: {fireJob}");
        }

        if (fireJob)
        {
	        Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Running JobBase");

            await this.Run(cancelToken).ConfigureAwait(false);

            // if the job errors, we will keep trying
            this.LastRunTime = DateTimeOffset.UtcNow;
        }
    }


    protected abstract Task Run(CancellationToken cancelToken);
    protected JobInfo JobInfo { get; private set; }

    /// <summary>
    /// Last runtime of this job.  Null if never run before.
    /// This value will persist between runs, it is not recommended that you set this yourself
    /// </summary>
    DateTimeOffset? lastRunTime;
    public DateTimeOffset? LastRunTime
    {
        get => this.lastRunTime;
        set => this.Set(ref this.lastRunTime, value);
    }


    /// <summary>
    /// Sets a minimum time between this job firing
    /// CAREFUL: jobs tend to NOT run when the users phone is not in use
    /// This value will persist between runs
    /// </summary>
    TimeSpan? minTime;
    public TimeSpan? MinimumTime
    {
        get => this.minTime;
        set => this.Set(ref this.minTime, value);
    }
}

