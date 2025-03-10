using GongSolutions.Wpf.DragDrop;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuizPageViewModel : ViewModelBase, IDropTarget
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDropTarget _dropHandler;
	private readonly IParameterNavigationService<Guid?, TrainingViewModel> _trainingNavigationService;
	private readonly IParameterNavigationService<Guid?, QuestionPageViewModel> _questionNavigationService;

	private Guid? _quizId;
	private Quiz? _quiz;

	private QuizViewModel? _quizViewModel;

	public QuizViewModel? Quiz
	{
		get => _quizViewModel;
		set
		{
			_quizViewModel = value;
			OnPropertyChanged();
		}
	}

	private bool _isNowSaving;

	public bool IsNowSaving
	{
		get => _isNowSaving;
		set
		{
			_isNowSaving = value;
			OnPropertyChanged();
		}
	}


	public ICommand OpenDropDownQuestionTypesCommand { get; }

	public IAsyncCommand<object?> StartQuizCommand { get; }
	public IAsyncCommand<object?> LoadQuizCommand { get; }
	public IAsyncCommand<object?> SaveQuizCommand { get; }
	public IAsyncCommand<object?> AddNewQuestionCommand { get; }
	public IAsyncCommand<object?> DeleteQuestionCommand { get; }
	public IAsyncCommand<object?> EditQuestionCommand { get; }

	public QuizPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Guid?, TrainingViewModel> trainingNavigationService,
		IParameterNavigationService<Guid?, QuestionPageViewModel> questionNavigationService)
	{
		_unitOfWork = unitOfWork;
		_dropHandler = new DefaultDropHandler();
		_trainingNavigationService = trainingNavigationService;
		_questionNavigationService = questionNavigationService;

		_quizId = id;
		_isNowSaving = false;

		LoadQuizCommand = AsyncDelegateCommand.Create(LoadQuizAsync);
		SaveQuizCommand = AsyncDelegateCommand.Create(SaveQuizAsync, (o) => Quiz != null);
		StartQuizCommand = AsyncDelegateCommand.Create(StartQuizAsync, CanStartQuiz);
		AddNewQuestionCommand = AsyncDelegateCommand.Create(AddNewQuestionAsync, CanAddNewQuestion);
		EditQuestionCommand = AsyncDelegateCommand.Create(OpenEditQuestionPageAsync);
		DeleteQuestionCommand = AsyncDelegateCommand.Create(DeleteQuestionAsync);
		
		OpenDropDownQuestionTypesCommand = new DelegateCommand(OpenDropDownQuestionTypes, CanAddNewQuestion);
	}

	public void Drop(IDropInfo dropInfo)
	{
		_dropHandler.Drop(dropInfo);
		ChangeAllQuestionNumbers();
	}

	public void DragOver(IDropInfo dropInfo) => _dropHandler.DragOver(dropInfo);

	private void ChangeAllQuestionNumbers()
	{
		if (Quiz == null)
		{
			return;
		}

		for (int i = 0; i < Quiz.Questions.Count(); i++)
		{
			Quiz.Questions.ElementAt(i).ListNumber = i + 1;
		}
	}

	private async Task StartQuizAsync(CancellationToken token)
	{
		if (SaveQuizCommand.CanExecute(null) && Quiz != null)
		{
			await SaveQuizCommand.ExecuteAsync(null);

			_trainingNavigationService.Navigate(Quiz.Id);
		}
	}

	private bool CanStartQuiz(object? obj) => Quiz != null && Quiz.Questions.Any();

	private async Task LoadQuizAsync(CancellationToken token)
	{
		if (_quizId is Guid id)
		{
			_quiz = await _unitOfWork.QuizRepository.GetByIdAsync(id, token: token);
		}
		else
		{
			_quiz = new Quiz();
		}

		_quizId = _quiz.Id;
		Quiz = new QuizViewModel(_quiz);
		Quiz.Questions = new ObservableCollection<QuestionViewModel>(
			Quiz.Questions.OrderBy(qVM => qVM.ListNumber));
	}

	private async Task SaveQuizAsync(CancellationToken token)
	{
		if (Quiz != null && _quiz != null)
		{
			IsNowSaving = true;

			Quiz.CopyToQuiz(_quiz);

			await _unitOfWork.QuizRepository.UpdateOrCreateAsync(_quiz, token);

			foreach (var questionVM in Quiz.Questions)
			{
				var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionVM.Id);
				questionVM.CopyToQuestion(question);
				await _unitOfWork.QuestionRepository.UpdateAsync(question, token);
			}

			await _unitOfWork.SaveAsync(token);

			IsNowSaving = false;
		}
	}

	private async Task OpenEditQuestionPageAsync(object? parameter, CancellationToken token)
	{
		if (SaveQuizCommand.CanExecute(null) &&
			parameter is QuestionViewModel questionViewModel)
		{
			await SaveQuizCommand.ExecuteAsync(null);

			_questionNavigationService.Navigate(questionViewModel.Id);
		}
	}

	private async Task DeleteQuestionAsync(object? obj, CancellationToken token)
	{
		if (obj is QuestionViewModel questionViewModel && Quiz != null)
		{
			var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionViewModel.Id);
			await _unitOfWork.QuestionRepository.DeleteAsync(question);
			await _unitOfWork.SaveAsync(token);

			Quiz.Questions = new ObservableCollection<QuestionViewModel>(
				Quiz.Questions.Where(qVM => qVM.Id != questionViewModel.Id));

			ChangeAllQuestionNumbers();
		}
	}

	private async Task AddNewQuestionAsync(object? obj, CancellationToken token)
	{
		var minimalNumber = 1;

		if (obj is QuestionType questionType && _quiz != null && Quiz != null)
		{
			var listNumber = (Quiz.Questions.LastOrDefault()?.ListNumber + 1) ?? minimalNumber;
			var question = new Question(_quizId, 1, questionType, listNumber);

			await _unitOfWork.QuestionRepository.CreateAsync(question, token);
			await _unitOfWork.SaveAsync(token);

			Quiz.Questions = new ObservableCollection<QuestionViewModel>(
				Quiz.Questions.Append(new QuestionViewModel(question)));
		}
	}

	private void OpenDropDownQuestionTypes(object? obj)
	{
		// ContextMenu located not in visual tree, thus "Binding" won't work
		if (obj is Button btn)
		{
			btn.ContextMenu.IsOpen = true;
		}
	}

	private bool CanAddNewQuestion(object? obj) => Quiz != null;
}
