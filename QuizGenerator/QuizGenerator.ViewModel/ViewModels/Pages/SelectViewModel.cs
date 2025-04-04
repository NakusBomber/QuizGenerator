using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class SelectViewModel : ViewModelBase
{
	private readonly IUnitOfWork _unitOfWork;

	private ObservableCollection<Quiz> _quizes = new();

	public ObservableCollection<Quiz> Quizes
	{
		get => _quizes;
		set
		{
			_quizes = value;
			OnPropertyChanged();
		}
	}

	private string? _searchText;

	public string? SearchText
	{
		get => _searchText;
		set
		{
			_searchText = value;
			OnPropertyChanged();

			SearchCommand.Execute(null);
		}
	}


	public IAsyncCommand<object?> SearchCommand { get; }
	public ICommand ClearSearchTextCommand { get; }
	public ICommand QuizNavigateCommand { get; }

	public SelectViewModel(
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Guid?, QuizPageViewModel> quizParameterNavigationService)
	{
		_unitOfWork = unitOfWork;

		QuizNavigateCommand = new ParameterNavigateCommand<Guid?, QuizPageViewModel>(quizParameterNavigationService);
		ClearSearchTextCommand = new DelegateCommand(ClearSearchText, CanClearSearchText);
		SearchCommand = AsyncDelegateCommand.Create(SearchQuizesAsync);
	}

	private async Task SearchQuizesAsync(CancellationToken token)
	{
		await Task.Run(async () =>
		{
			var quizes = await _unitOfWork.QuizRepository.GetAsync(
				q => q.Name.Contains(SearchText ?? string.Empty),
				token: token);

			Application.Current.Dispatcher.Invoke(() =>
			{
				Quizes = new ObservableCollection<Quiz>(quizes);
			});
		}, token);
	}

	private void ClearSearchText(object? parameter) => SearchText = string.Empty;

	private bool CanClearSearchText(object? obj) => SearchText != null && SearchText.Length > 0;
}
