using MyPrismApp1.Jobs;
using Shiny.Jobs;

namespace MyPrismApp1.ViewModels;

public class MainPageViewModel : BindableBase
{
	private ISemanticScreenReader _screenReader { get; }
	private int _count;
	private IJobManager _jobManager;

	public MainPageViewModel(ISemanticScreenReader screenReader, IJobManager jobManager)
	{
		_screenReader = screenReader;
		CountCommand = new DelegateCommand(OnCountCommandExecuted);
		StartJobCommand = new DelegateCommand(OnStartJobCommandExecuted);
		_jobManager = jobManager;
	}

	public string Title => "Main Page";

	private string _text = "Click me";
	public string Text
	{
		get => _text;
		set => SetProperty(ref _text, value);
	}

	public DelegateCommand CountCommand { get; }

	private async void OnCountCommandExecuted()
	{
		_count++;
		if (_count == 1)
			Text = "Clicked 1 time";
		else if (_count > 1)
			Text = $"Clicked {_count} times";

		_screenReader.Announce(Text);
	}
	public DelegateCommand StartJobCommand { get; }

	private async void OnStartJobCommandExecuted()
	{
		await MyJob.RunJob(EventTriggers.OnUserInput);
	}

	private string _startJobButtonText = "Start Job";
	public string StartJobButtonText
	{
		get => _startJobButtonText;
		set => SetProperty(ref _startJobButtonText, value);
	}

}
