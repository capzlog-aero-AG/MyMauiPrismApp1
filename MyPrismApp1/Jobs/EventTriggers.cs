using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Jobs;

namespace MyPrismApp1.Jobs
{
	public enum EventTriggers
	{
		Repeating,
		OnCreated,
		OnResume,
		OnStopped,
		OnActivated,
		OnUserInput,
		OnLogin,
		OnSignUp
	}
}
