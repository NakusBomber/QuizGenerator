using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.Other;
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
	public IAsyncCommand<object?> FinishCommand { get; }
	public ICommand NextQuestionCommand { get; }
	public ICommand SubmitAnswerCommand { get; }
	public ICommand StartTimerCommand { get; }
	public ICommand StopTimerCommand { get; }

	public TrainingViewModel(
		Guid? id,
		IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_timer = CreateTimer();

		_quizId = id;
		
		LoadQuizCommand = AsyncDelegateCommand.Create(LoadQuizAsync);
		FinishCommand = AsyncDelegateCommand.Create(FinishQuizAsync);
		NextQuestionCommand = new DelegateCommand(NextQuestion, CanNextQuestion);
		SubmitAnswerCommand = new DelegateCommand(SubmitAnswer);
		StartTimerCommand = new DelegateCommand(StartTimer);
		StopTimerCommand = new DelegateCommand(StopTimer);
	}

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
			// TODO : Make another way
		}

		await Task.Run(async () =>
		{
			if (_quizId is Guid id)
			{
				_quiz = await _unitOfWork.QuizRepository.GetByIdAsync(id, token);
			}

			// TODO : Make another way

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

		if (NextQuestionCommand.CanExecute(null))
		{
			NextQuestionCommand.Execute(null);
		}
		if (Quiz != null && Quiz.IsNeedInterval && StartTimerCommand.CanExecute(null))
		{
			StartTimerCommand.Execute(null);
		}
	}

	private async Task FinishQuizAsync(CancellationToken token)
	{
		MessageBox.Show("FINISH");
	}

	private void NextQuestion(object? parameter)
	{
		if (Quiz == null)
		{
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
	}

	private bool CanNextQuestion(object? obj) =>
		Quiz != null &&
		_questionIndex < Quiz.Questions.Count;

	private void SubmitAnswer(object? parameter)
	{
		// TODO : Make logic
	}

	private DispatcherTimer CreateTimer()
	{
		var sec = TimeSpan.FromSeconds(1);
		return new DispatcherTimer(
			sec,
			DispatcherPriority.Normal,
			(s, e) =>
			{
				if (Quiz == null || Quiz.Interval == null)
				{
					return;
				}

				if (Quiz.Interval == TimeSpan.Zero)
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

				Quiz.Interval -= sec;
			},
			Application.Current.Dispatcher)
		{
			IsEnabled = false
		};
	}
}
