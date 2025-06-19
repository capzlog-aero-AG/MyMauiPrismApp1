using System.Diagnostics;
using System.Runtime;
using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Jobs;
using UriTypeConverter = Microsoft.Maui.Controls.UriTypeConverter;

namespace MyPrismApp1.Jobs;

public class MyJob : JobBase
{
	//readonly INotificationManager notificationManager;

	MyJob()
	{
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: Constructor {nameof(MyJob)}");
	}

	protected override async Task Run(CancellationToken cancelToken)
	{
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Started - {this.JobInfo.Identifier}");
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Key1: {this.JobInfo.Parameters["Key1"]}");
		await Task.Delay(50, cancelToken);
		Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Ended - {this.JobInfo.Identifier}");

		await Task.Delay(1000, cancelToken).ConfigureAwait(false);
	}
}