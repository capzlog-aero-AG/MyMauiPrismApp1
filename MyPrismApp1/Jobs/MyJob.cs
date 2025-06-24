using System.Diagnostics;
using System.Runtime;
using System.Threading;
using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Jobs;

namespace MyPrismApp1.Jobs;

public class MyJob : IJob
{
	private static readonly TimeSpan MinimumTimeBetweenTwoAutomaticStarts = TimeSpan.FromMinutes(3);

	private static DateTime _lastExecutionStart = new DateTime(2000, 1, 1); // Do not use MinValue, otherwise the first update results in a value below the minimum possible because of the usage of UTC
	private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

	public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
	{
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Run called on Thread {Thread.CurrentThread.Name} from {nameof(EventTriggers)}: {jobInfo.Parameters[nameof(EventTriggers)]}");

		if (!await MyJob.Semaphore.WaitAsync(TimeSpan.Zero, cancelToken))
		{
			Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} is already running");
			return;
		}
		try
		{

			// Check if the job should start automatically
			var value = jobInfo.Parameters[nameof(EventTriggers)];
			EventTriggers.TryParse(value, out EventTriggers eventTrigger);

			if (!eventTrigger.IsForcedStart())
			{
				var earliestNextStart = _lastExecutionStart.Add(MinimumTimeBetweenTwoAutomaticStarts);
				if (DateTime.UtcNow < earliestNextStart)
				{
					Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: Job '{jobInfo.Identifier}' last run at {_lastExecutionStart:dd.MM.yyyy HH:mm:ss}z and will not be started automatically again until {earliestNextStart:dd.MM.yyyy HH:mm:ss}z");
					return;
				}
			}

			await Task.Delay(5000, cancelToken).ConfigureAwait(false);
		}
		finally
		{
			MyJob.Semaphore.Release();
		}
		_lastExecutionStart = DateTime.UtcNow;  // Reset it at the end as well, even though it was reset in the beginning, because the task might take a significant time
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} END on Thread {Thread.CurrentThread.Name}");
		jobInfo.Parameters[nameof(EventTriggers)] = EventTriggers.Repeating.ToString();
	}

	/// <summary>
	/// We should use this method to run the job
	/// </summary>
	/// <param name="jobInfo"></param>
	/// <returns></returns>
	public static async Task RunJob(EventTriggers eventTrigger)
	{
		await RunJob(JobExtensions.DefaultJobInfoFromEventTrigger(eventTrigger));
	}

	private static async Task RunJob(JobInfo? jobInfo = null)
	{
		var jobManager = ServiceHelper.GetService<IJobManager>();

		var job = jobManager.GetJob(nameof(MyJob));

		if (job == null)
		{
			// If the job is not yet registered by name, do so.
			jobManager.Register(jobInfo ?? JobExtensions.DefaultJobInfo);
		}
		var isJobInfoEqualToRegisteredJob = job?.Parameters?.SequenceEqual(jobInfo.Parameters) == true;

		if (jobInfo == null || isJobInfoEqualToRegisteredJob)
		{
			// No new information provided, so just start the already registered job
			await jobManager.Run(nameof(MyJob));
			return;
		}

		// There are changed parameters, so cancel, register and run the new job

		jobManager.Cancel(nameof(MyJob));
		jobManager.Register(jobInfo);
		await jobManager.Run(nameof(MyJob));
	}

}