using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Jobs;

namespace MyPrismApp1.Jobs
{
	public static class JobExtensions
	{
		public static JobInfo DefaultJobInfo =>
			new Shiny.Jobs.JobInfo(
				nameof(MyJob),
				typeof(MyJob),
				true,
				new Dictionary<string, string>()
				{
					{nameof(EventTriggers), nameof(EventTriggers.Repeating)},
				},
				InternetAccess.Any,
				false, // do not require device to be plugged in
				false, // do not require device to have high battery
				false); // system jobs and jobs with JobType == null will be cancelled and restarted upon Job Start by Shiny (not yet clear what we need here)

		public static JobInfo DefaultJobInfoFromEventTrigger(EventTriggers eventTrigger)
		{
			var jobInfo = DefaultJobInfo;
			jobInfo.Parameters![nameof(EventTriggers)] = eventTrigger.ToString();

			return jobInfo;
		}

	}
}
