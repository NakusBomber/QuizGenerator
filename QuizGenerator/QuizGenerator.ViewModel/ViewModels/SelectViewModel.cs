using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels;

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

			if (SearchCommand.CanExecute(null))
			{
				SearchCommand.Execute(null);
			}
		}
	}


	public IAsyncCommand<object?> SearchCommand { get; }

	public SelectViewModel(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

		SearchCommand = AsyncDelegateCommand.Create(SearchQuizesAsync);
	}

	private async Task SearchQuizesAsync(CancellationToken token)
	{
		var quizes = await _unitOfWork.QuizRepository.GetAsync(q => q.Name.Contains(SearchText ?? string.Empty));
		Quizes = new ObservableCollection<Quiz>(quizes);
	}
}
