using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
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
	private readonly IUnitOfWork _unitOfWork;

	private ObservableCollection<Quiz> _recentlyQuizes = new();
	public ObservableCollection<Quiz> RecentlyQuizes
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
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Quiz?> quizParameterNavigationService,
		INavigationService selectNavigationService)
	{
		_unitOfWork = unitOfWork;

		QuizNavigateCommand = new ParameterNavigateCommand<Quiz?>(quizParameterNavigationService);
		SelectNavigateCommand = new NavigateCommand(selectNavigationService);

		LoadQuizesCommand = AsyncDelegateCommand.Create(LoadQuizesAsync);
	}

	private async Task LoadQuizesAsync(CancellationToken token)
	{
		var list = await _unitOfWork.QuizRepository.GetAsync();
		RecentlyQuizes = new ObservableCollection<Quiz>(list);
	}
}
