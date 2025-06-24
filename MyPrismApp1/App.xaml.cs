using MyPrismApp1.Jobs;
using Shiny.Jobs;

namespace MyPrismApp1
{
    public partial class App : Application
    {
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState activationState)
		{
			Window window = base.CreateWindow(activationState);

			window.Created += async (s, e) =>
			{
				await MyJob.RunJob(EventTriggers.OnCreated);
			};
			window.Resumed += async (s, e) =>
			{
				await MyJob.RunJob(EventTriggers.OnResume);
			};
			window.Activated += async (s, e) =>
			{
				await MyJob.RunJob(EventTriggers.OnActivated);
			};

			window.Stopped += async (s, e) =>
			{
				await MyJob.RunJob(EventTriggers.OnStopped);
			};

			return window;
		}


	}
}