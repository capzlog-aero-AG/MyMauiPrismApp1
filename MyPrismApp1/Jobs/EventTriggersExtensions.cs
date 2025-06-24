using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Jobs;

namespace MyPrismApp1.Jobs
{
	public static class EventTriggersExtensions
	{
		public static bool IsForcedStart(this EventTriggers eventTrigger)
		{
			return eventTrigger == EventTriggers.OnLogin || 
			       eventTrigger == EventTriggers.OnSignUp || 
			       eventTrigger == EventTriggers.OnUserInput;
		}
	}
}
