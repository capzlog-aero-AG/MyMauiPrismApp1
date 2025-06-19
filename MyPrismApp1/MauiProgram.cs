using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using MyPrismApp1.Jobs;
using MyPrismApp1.Views;
using Shiny;
using Shiny.Jobs;

namespace MyPrismApp1
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseShiny()
				.UseMauiCompatibility() // TODO-MAUI: What is this? Found in the sample PrismFullNavigation
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.ConfigureMauiHandlers(handlers =>
				{
					// TODO-MAUI: What is this? Found in the sample PrismFullNavigation
				})
				.UsePrism(prism =>
				{
					// TODO-MAUI: What are module catalogs and do we need this?
					prism.ConfigureModuleCatalog(moduleCatalog =>
						{
							//moduleCatalog.AddModule<MauiAppModule>();
							//moduleCatalog.AddModule<MauiTestRegionsModule>();
						});

					// TODO-MAUI: What shall be registered in IServiceCollection and what in the Prism containerRegistry?
					prism.ConfigureServices(services =>
					{
						// Register services with the IServiceCollection
						//services.AddSingleton<IFoo, Foo>();
					});
					prism.RegisterTypes(containerRegistry =>
					{
						containerRegistry.RegisterForNavigation<MainPage>();

						containerRegistry.RegisterInstance(SemanticScreenReader.Default);
					});

					prism.ConfigureLogging(builder =>
					{
						builder.AddDebug(); // TODO-MAUI: Shall this be here and/or below as it was in the MAUI template?
					});

					// AddGlobalNavigationObserver is in the package Prism.Maui.Rx
					//prism.AddGlobalNavigationObserver(context => context.Subscribe(x =>
					//{
					//	if (x.Type == NavigationRequestType.Navigate)
					//		Console.WriteLine($"Navigation: {x.Uri}");
					//	else
					//		Console.WriteLine($"Navigation: {x.Type}");

					//	var status = x.Cancelled ? "Cancelled" : x.Result.Success ? "Success" : "Failed";
					//	Console.WriteLine($"Result: {status}");

					//	if (status == "Failed" && !string.IsNullOrEmpty(x.Result?.Exception?.Message))
					//		Console.Error.WriteLine(x.Result.Exception.Message);
					//}));

					prism.OnInitialized(container =>
					{
						// resolve services and do other initialization

						// TODO-MAUI: Sample code from the PrismFullNavigation sample
						//Debug.WriteLine("MAUI BUILDER - OnInitialized IContainerRegistry");

						//var eventAggregator = obj.Resolve<IEventAggregator>();
						//eventAggregator?.GetEvent<NavigationRequestEvent>().Subscribe(context => {

						//	Debug.WriteLine("\nNAVIGATIONSERVICE");
						//	Debug.WriteLine("Uri = " + context.Uri);
						//	Debug.WriteLine("Parameters = " + context.Parameters);
						//	Debug.WriteLine("Type = " + context.Type);
						//	Debug.WriteLine("Cancelled = " + context.Cancelled);
						//	Debug.WriteLine("NAVIGATIONSERVICE RESULT");
						//	Debug.WriteLine("Success = " + context.Result.Success);
						//	Debug.WriteLine("Context = " + context.Result.Context);

						//	var exc = context.Result.Exception;
						//	if (exc != null)
						//	{
						//		Debug.WriteLine("NAVIGATIONSERVICE EXCEPTION");
						//		Debug.WriteLine("Exception = " + exc);
						//		Debug.WriteLine("Data = " + exc.Data);
						//		Debug.WriteLine("HelpLink = " + exc.HelpLink);
						//		Debug.WriteLine("HResult = " + exc.HResult);
						//		Debug.WriteLine("InnerException = " + exc.InnerException);
						//		Debug.WriteLine("Message = " + exc.Message);
						//		Debug.WriteLine("Source = " + exc.Source);
						//		Debug.WriteLine("StackTrace = " + exc.StackTrace);
						//		Debug.WriteLine("TargetSite = " + exc.TargetSite);
						//	}


						//});
					})
					.OnInitialized(() =>
					{
						// do some initialization that doesn't require resolving services
					});

					prism.CreateWindow(async navigationService =>
					{
						var navResult = await navigationService.NavigateAsync("/" + nameof(NavigationPage) + "/" + nameof(MainPage));
					});
				})
				.RegisterShinyServices();

#if DEBUG
			// TODO-MAUI: Shall this be here and/or above in the prism configuration?
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
		static MauiAppBuilder RegisterShinyServices(this MauiAppBuilder builder)
		{
			var s = builder.Services;

			Console.WriteLine($"TODONOW-PM: Registering {nameof(MyJob)}");

			s.AddJob(new Shiny.Jobs.JobInfo(nameof(MyJob), typeof(MyJob), 
				true, 
				new Dictionary<string, string>()
				{
					{"Key1", "Passed Parameter 1"}
				}, 
				InternetAccess.Any, 
				false, // do not require device to be plugged in
				false, // do not require device to have high battery
				false)); // system jobs and jobs with JobType == null will be cancelled and restarted upon Job Start by Shiny (not yet clear what we need here)
			
			return builder;
		}
	}
}
