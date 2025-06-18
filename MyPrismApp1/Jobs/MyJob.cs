using System.Diagnostics;
using Shiny;
using Shiny.Jobs;
namespace MyPrismApp1.Jobs;

public class MyJob : IJob
{
	//readonly INotificationManager notificationManager;

	MyJob()
	{
		Console.WriteLine($"TODONOW-PM: Constructor {nameof(MyJob)}");
	}
	public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
	{
		//Trace.WriteLine($"{nameof(MyJob)} Started - {jobInfo.Identifier}");
		Console.WriteLine($"TODONOW-PM: {nameof(MyJob)} Started - {jobInfo.Identifier}");
		await Task.Delay(50, cancelToken);
		//Trace.WriteLine($"{nameof(MyJob)} Ended - {jobInfo.Identifier}");
		Console.WriteLine($"TODONOW-PM: {nameof(MyJob)} Ended - {jobInfo.Identifier}");

		await Task.Delay(1000, cancelToken).ConfigureAwait(false);
	}
}