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
				await RunJob(nameof(Window.Created));
			};
			window.Resumed += async (s, e) =>
			{
				await RunJob(nameof(Window.Resumed));
			};
			window.Activated += async (s, e) =>
			{
				await RunJob(nameof(Window.Activated));
			};

			window.Stopped += async (s, e) =>
			{
				await RunJob(nameof(Window.Stopped));
			};

			return window;
		}

		private async Task RunJob(string eventName)
		{
			var jobManager = ServiceHelper.GetService<IJobManager>();
			Console.WriteLine($"TODONOW-PM {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss")}: {nameof(MyJob)} Triggered by event: {eventName}");
			await jobManager.Run(nameof(MyJob));
		}
	}
}