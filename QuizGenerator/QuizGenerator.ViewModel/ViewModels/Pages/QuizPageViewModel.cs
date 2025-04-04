using GongSolutions.Wpf.DragDrop;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using QuizGenerator.ViewModel.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class QuizPageViewModel : SavingStateViewModel, IDropTarget
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDropTarget _dropHandler;
	private readonly IParameterNavigationService<Guid?, TrainingViewModel> _trainingNavigationService;
	private readonly IParameterNavigationService<Guid?, QuestionPageViewModel> _questionNavigationService;
	private readonly IBackNavigationService _backNavigationService;
	private readonly IWindowNavigationService<ConfirmationWindowViewModel, bool> _confirmationNavigationService;

	private readonly PropertyChangedEventHandler _propertyChangedHandler;
	private readonly ICollection<Question> _newQuestions;
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


	public ICommand OpenDropDownQuestionTypesCommand { get; }

	public IAsyncCommand<object?> StartQuizCommand { get; }
	public IAsyncCommand<object?> LoadQuizCommand { get; }
	public IAsyncCommand<object?> SaveQuizCommand { get; }
	public IAsyncCommand<object?> DeleteQuizCommand { get; }
	public IAsyncCommand<object?> AddNewQuestionCommand { get; }
	public IAsyncCommand<object?> DeleteQuestionCommand { get; }
	public IAsyncCommand<object?> EditQuestionCommand { get; }

	public QuizPageViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IParameterNavigationService<Guid?, TrainingViewModel> trainingNavigationService,
		IParameterNavigationService<Guid?, QuestionPageViewModel> questionNavigationService,
		IBackNavigationService backNavigationService,
		IWindowNavigationService<ConfirmationWindowViewModel, bool> confirmationNavigationService)
	{
		_unitOfWork = unitOfWork;
		_dropHandler = new DefaultDropHandler();
		_trainingNavigationService = trainingNavigationService;
		_questionNavigationService = questionNavigationService;
		_backNavigationService = backNavigationService;
		_confirmationNavigationService = confirmationNavigationService;

		_propertyChangedHandler = (s, a) => IsNeedSaving = true;
		_quizId = id;
		_newQuestions = new List<Question>();

		LoadQuizCommand = AsyncDelegateCommand.Create(LoadQuizAsync);
		SaveQuizCommand = AsyncDelegateCommand.Create(SaveQuizAsync, o => Quiz != null && IsNeedSaving);
		DeleteQuizCommand = AsyncDelegateCommand.Create(DeleteQuizAsync, o => Quiz != null);
		StartQuizCommand = AsyncDelegateCommand.Create(StartQuizAsync, CanStartQuiz);
		AddNewQuestionCommand = AsyncDelegateCommand.Create(AddNewQuestionAsync, CanAddNewQuestion);
		EditQuestionCommand = AsyncDelegateCommand.Create(OpenEditQuestionPageAsync);
		DeleteQuestionCommand = AsyncDelegateCommand.Create(DeleteQuestionAsync);
		
		OpenDropDownQuestionTypesCommand = new DelegateCommand(OpenDropDownQuestionTypes, CanAddNewQuestion);

	}

	~QuizPageViewModel()
	{
		if (Quiz != null)
		{
			Quiz.PropertyChanged -= _propertyChangedHandler;
		}
	}

	public void Drop(IDropInfo dropInfo)
	{
		_dropHandler.Drop(dropInfo);
		ChangeAllQuestionNumbers();
		IsNeedSaving = true;
		CommandManager.InvalidateRequerySuggested();
	}

	public void DragOver(IDropInfo dropInfo) => _dropHandler.DragOver(dropInfo);

	private void ChangeAllQuestionNumbers()
	{
		if (Quiz == null)
		{
			return;
		}

		for (int i = 0; i < Quiz.Questions.Count; i++)
		{
			Quiz.Questions.ElementAt(i).ListNumber = i + 1;
		}
	}

	private async Task StartQuizAsync(CancellationToken token)
	{
		if (SaveQuizCommand.CanExecute(null))
		{
			await SaveQuizCommand.ExecuteAsync(null);
		}

		if (Quiz != null)
		{
			_trainingNavigationService.Navigate(Quiz.Id);
		}
	}

	private bool CanStartQuiz(object? obj) => Quiz != null && Quiz.Questions.Any();

	private async Task LoadQuizAsync(CancellationToken token)
	{
		await Task.Run(async () =>
		{
			if (_quizId is Guid id)
			{
				_quiz = await _unitOfWork.QuizRepository.GetByIdAsync(id, token);
			}
			else
			{
				_quiz = new Quiz();
			}

			_quizId = _quiz.Id;

			var questionViewModels = _quiz.Questions
				.ToVMs()
				.OrderBy(q => q.ListNumber);

			Application.Current.Dispatcher.Invoke(() =>
			{
				Quiz = new QuizViewModel(_quiz);
				Quiz.Questions = new ObservableCollection<QuestionViewModel>(questionViewModels);

				Quiz.PropertyChanged += _propertyChangedHandler;
			});

		}, token);
	}

	private async Task SaveQuizAsync(CancellationToken token)
	{
		if (Quiz == null || _quiz == null)
		{
			return;
		}

		IsNowSaving = true;

		await Task.Run(async () =>
		{
			Quiz.DateTimeChanged = DateTime.Now;
			Quiz.CopyToQuiz(_quiz);

			await _unitOfWork.QuizRepository.UpdateOrCreateAsync(_quiz, token);

			
			foreach (var questionVM in Quiz.Questions)
			{
				if (_newQuestions.FirstOrDefault(q => q.Id == questionVM.Id) is Question newQuestion)
				{
					questionVM.CopyToQuestion(newQuestion);
					await _unitOfWork.QuestionRepository.CreateAsync(newQuestion, token);
				}
				else
				{
					var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionVM.Id, token);
					questionVM.CopyToQuestion(question);
					await _unitOfWork.QuestionRepository.UpdateAsync(question, token);
				}
			}

			_newQuestions.Clear();
			await _unitOfWork.SaveAsync(token);
		}, token);

		IsNowSaving = false;
		IsNeedSaving = false;
	}

	private async Task DeleteQuizAsync(CancellationToken token)
	{
		if (Quiz == null || _quiz == null)
		{
			return;
		}

		var result = _confirmationNavigationService.Navigate(
			ConfirmationWindowViewModel.DeletePrefab(Quiz.Name));

		if (!result)
		{
			return;
		}

		try
		{
			var quiz = await _unitOfWork.QuizRepository.GetByIdAsync(Quiz.Id, token);
			await _unitOfWork.QuizRepository.DeleteAsync(quiz, token);

			await _unitOfWork.SaveAsync(token);
		}
		catch (InvalidOperationException)
		{
		}
		finally
		{
			_backNavigationService.Navigate();
		}
	}

	private async Task OpenEditQuestionPageAsync(object? parameter, CancellationToken token)
	{
		if (SaveQuizCommand.CanExecute(null))
		{
			await SaveQuizCommand.ExecuteAsync(null);
		}

		if (parameter is QuestionViewModel questionViewModel)
		{
			_questionNavigationService.Navigate(questionViewModel.Id);
		}
	}

	private async Task DeleteQuestionAsync(object? obj, CancellationToken token)
	{
		if (obj is QuestionViewModel questionViewModel && Quiz != null)
		{
			if (_newQuestions.FirstOrDefault(q => q.Id == questionViewModel.Id) is Question newQuestion)
			{
				_newQuestions.Remove(newQuestion);
			}
			else
			{
				var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionViewModel.Id, token);
				await _unitOfWork.QuestionRepository.DeleteAsync(question, token);
			}

			Quiz.Questions.Remove(questionViewModel);
			
			ChangeAllQuestionNumbers();
			IsNeedSaving = true;
		}
	}

	private Task AddNewQuestionAsync(object? obj, CancellationToken token)
	{
		var minimalNumber = 1;

		if (obj is QuestionType questionType && _quiz != null && Quiz != null)
		{
			var listNumber = Quiz.Questions.LastOrDefault()?.ListNumber + 1 ?? minimalNumber;
			var question = new Question(_quiz.Id, 1, questionType, listNumber);

			_newQuestions.Add(question);

			Quiz.Questions.Add(new QuestionViewModel(question));
			IsNeedSaving = true;
		}
		return Task.CompletedTask;
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
