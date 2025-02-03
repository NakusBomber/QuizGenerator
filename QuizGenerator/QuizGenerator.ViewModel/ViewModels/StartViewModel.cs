using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class StartViewModel : ViewModelBase
{
	private ObservableCollection<string> _recentlyQuizes = new();
	public ObservableCollection<string> RecentlyQuizes
	{
		get => _recentlyQuizes;
		set
		{
			_recentlyQuizes = value;
			OnPropertyChanged();
		}
	}

	public ICommand SelectNavigateCommand { get; }
	public ICommand QuizNavigateCommand { get; }
	public IAsyncCommand<object?> LoadQuizesCommand { get; }
	public StartViewModel(
		INavigationService quizNavigationService,
		INavigationService selectNavigationService)
	{
		QuizNavigateCommand = new NavigateCommand(quizNavigationService);
		SelectNavigateCommand = new NavigateCommand(selectNavigationService);

		LoadQuizesCommand = AsyncDelegateCommand.Create(TempAsync);
	}

	private async Task TempAsync(CancellationToken token)
	{
		await Task.Delay(2000, token);
		RecentlyQuizes = new ObservableCollection<string>(["First", "Last"]);
	}
}
