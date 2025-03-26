using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class TrainingViewModel : ViewModelBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly DispatcherTimer _timer;
	private readonly IParameterNavigationService<TrainingViewModel, AnalisysViewModel> _analisysNavigationService;
	private readonly IUserAnswerEvaluator _userAnswerEvaluator;
	
	private Guid? _quizId;
	private Quiz? _quiz;
	private int _questionIndex = 0;

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

	private TimeSpan? _timeLeft;

	public TimeSpan? TimeLeft
	{
		get => _timeLeft;
		set
		{
			_timeLeft = value;
			OnPropertyChanged();
		}
	}


	private QuestionViewModel? _activeQuestion;

	public QuestionViewModel? ActiveQuestion
	{
		get => _activeQuestion;
		set
		{
			_activeQuestion = value;
			OnPropertyChanged();
		}
	}

	public IAsyncCommand<object?> LoadQuizCommand { get; }
	public ICommand FinishCommand { get; }
	public ICommand NextQuestionCommand { get; }
	public ICommand StartTimerCommand { get; }
	public ICommand StopTimerCommand { get; }

	public TrainingViewModel(
		Guid? id,
		IUnitOfWork unitOfWork,
		IUserAnswerEvaluator userAnswerEvaluator,
		IParameterNavigationService<TrainingViewModel, AnalisysViewModel> analisysNavigationService)
	{
		_unitOfWork = unitOfWork;
		_analisysNavigationService = analisysNavigationService;
		_userAnswerEvaluator = userAnswerEvaluator;
		_timer = CreateTimer();

		_quizId = id;

		LoadQuizCommand = AsyncDelegateCommand.Create(LoadQuizAsync);
		FinishCommand = new DelegateCommand(FinishQuiz);
		NextQuestionCommand = new DelegateCommand(NextQuestion, CanNextQuestion);
		StartTimerCommand = new DelegateCommand(StartTimer);
		StopTimerCommand = new DelegateCommand(StopTimer);
	}

	public bool IsLastQuestion => Quiz != null && _questionIndex == Quiz.Questions.Count;

	private void StartTimer(object? obj)
	{
		if (_timer == null)
		{
			return;
		}
		
		_timer.Stop();
		_timer.Start();
	}

	private void StopTimer(object? obj)
	{
		if (_timer == null)
		{
			return;
		}
		
		_timer.Stop();
	}

	private async Task LoadQuizAsync(CancellationToken token)
	{
		if (_quizId == null)
		{
			return;
		}

		await Task.Run(async () =>
		{
			if (_quizId is Guid id)
			{
				try
				{
					_quiz = await _unitOfWork.QuizRepository.GetByIdAsync(id, token);
					_quiz.DateTimeLastPractice = DateTime.Now;
					await _unitOfWork.QuizRepository.UpdateAsync(_quiz);
				}
				catch (InvalidOperationException)
				{
					return;
				}
			}
			await _unitOfWork.SaveAsync(token);

			if (_quiz == null)
			{
				return;
			}

			var questionViewModels = _quiz.Questions
				.ToVMs()
				.OrderBy(q => q.ListNumber);

			Application.Current.Dispatcher.Invoke(() =>
			{
				Quiz = new QuizViewModel(_quiz)
				{
					Questions = new ObservableCollection<QuestionViewModel>(questionViewModels)
				};
			});
		}, token);

		_questionIndex = 0;
		ActiveQuestion = null;
		if (NextQuestionCommand.CanExecute(null))
		{
			NextQuestionCommand.Execute(null);
		}
		if (Quiz != null && Quiz.IsNeedInterval && StartTimerCommand.CanExecute(null))
		{
			TimeLeft = Quiz.Interval;
			StartTimerCommand.Execute(null);
		}
	}

	private void FinishQuiz(object? parameter)
	{
		if (StopTimerCommand.CanExecute(null))
		{
			StopTimerCommand.Execute(null);
		}

		_analisysNavigationService.Navigate(this);
	}

	private void NextQuestion(object? parameter)
	{
		if (Quiz == null)
		{
			return;
		}

		if (IsLastQuestion)
		{
			if (FinishCommand.CanExecute(null))
			{
				FinishCommand.Execute(null);
			}
			return;
		}
		
		var questionViewModel = Quiz.Questions.ElementAt(_questionIndex);
		foreach (var qd in questionViewModel.QuestionDetails)
		{
			qd.AnswerDetails.Shuffle();
		}
		questionViewModel.QuestionDetails.Shuffle();

		ActiveQuestion = questionViewModel;
		_questionIndex++;
		OnPropertyChanged(nameof(IsLastQuestion));
	}

	private bool CanNextQuestion(object? obj) =>
		Quiz != null &&
		(ActiveQuestion == null || _userAnswerEvaluator.Validate(ActiveQuestion));

	private DispatcherTimer CreateTimer()
	{
		var sec = TimeSpan.FromSeconds(1);
		return new DispatcherTimer(
			sec,
			DispatcherPriority.Normal,
			(s, e) =>
			{
				if (TimeLeft == null)
				{
					return;
				}

				if (TimeLeft == TimeSpan.Zero)
				{
					if (StopTimerCommand.CanExecute(null))
					{
						StopTimerCommand.Execute(null);
					}
					if (FinishCommand.CanExecute(null))
					{
						FinishCommand.Execute(null);
					}

					return;
				}

				TimeLeft -= sec;
			},
			Application.Current.Dispatcher)
		{
			IsEnabled = false
		};
	}
}
