using QuizGenerator.View.IndependentComponents.Models;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Models;
using QuizGenerator.ViewModel.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class AnalisysViewModel : ViewModelBase
{
	private readonly IBackNavigationService _backNavigationService;
	private readonly IUserAnswerEvaluator _userAnswerEvaluator;
	private readonly IWindowNavigationService<ConfirmationWindowViewModel, bool> _confirmationNavigationService;

	private string _quizName;

	public string QuizName
	{
		get => _quizName;
		set
		{
			_quizName = value;
			OnPropertyChanged();
		}
	}


	private TimeSpan? _elapsedTime;

	public TimeSpan? ElapsedTime
	{
		get => _elapsedTime;
		set
		{
			_elapsedTime = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsWasTime));
		}
	}

	private int _countCorrectAnswers;

	public int CountCorrectAnswers
	{
		get => _countCorrectAnswers;
		set
		{
			_countCorrectAnswers = value;
			OnScoreChanged();
			OnPropertyChanged();
		}
	}

	private int _countIncorrectAnswers;

	public int CountIncorrectAnswers
	{
		get => _countIncorrectAnswers;
		set
		{
			_countIncorrectAnswers = value;
			OnScoreChanged();
			OnPropertyChanged();
		}
	}

	private int _countPartiallyCorrectAnswers;

	public int CountPartiallyCorrectAnswers
	{
		get => _countPartiallyCorrectAnswers;
		set
		{
			_countPartiallyCorrectAnswers = value;
			OnScoreChanged();
			OnPropertyChanged();
		}
	}

	private ObservableCollection<AnalisysQuestionViewModel> _analisedQuestions;

	public ObservableCollection<AnalisysQuestionViewModel> AnalisedQuestions
	{
		get => _analisedQuestions;
		set
		{
			_analisedQuestions = value;
			OnPropertyChanged();
		}
	}


	private ObservableCollection<PieChartData> _pieChartData;

	public ObservableCollection<PieChartData> PieChartData
	{
		get => _pieChartData;
		set
		{
			_pieChartData = value;
			OnPropertyChanged();
		}
	}

	public ICommand RetryCommand { get; }
	public AnalisysViewModel(
		TrainingViewModel trainingViewModel,
		IWindowNavigationService<ConfirmationWindowViewModel, bool> confirmationNavigationService,
		IBackNavigationService backNavigationService)
	{
		_quizName = trainingViewModel.Quiz?.Name ?? string.Empty;
		_userAnswerEvaluator = new UserAnswerEvaluator();
		_confirmationNavigationService = confirmationNavigationService;
		_backNavigationService = backNavigationService;

		_analisedQuestions = new(trainingViewModel.Quiz?.Questions
			.Select(q => new AnalisysQuestionViewModel(q)) ?? []);

		SetIsReadOnly();

		ElapsedTime = trainingViewModel.Quiz?.Interval - trainingViewModel.TimeLeft;
		_pieChartData = new ObservableCollection<PieChartData>([
				new PieChartData("Correct", 0, Brushes.Green),
				new PieChartData("Partially correct", 0, Brushes.Yellow),
				new PieChartData("Incorrect", 0, Brushes.Red)]);

		CalculateAllQuestions();

		RetryCommand = new DelegateCommand(RetryPractice);
	}

	public bool IsWasTime => ElapsedTime != null;
	public float SumCurrentScore => AnalisedQuestions.Sum(a => a.Score);
	public int SumMaxScore => AnalisedQuestions.Sum(a => a.Question.EvaluationPrice);
	private void OnScoreChanged()
	{
		PieChartData.ElementAt(0).Weight = CountCorrectAnswers;
		PieChartData.ElementAt(1).Weight = CountPartiallyCorrectAnswers;
		PieChartData.ElementAt(2).Weight = CountIncorrectAnswers;
	}

	private void SetIsReadOnly()
	{
		if (AnalisedQuestions.Count == 0)
		{
			return;
		}

		AnalisedQuestions
			.SelectMany(aq => aq.Question.QuestionDetails)
			.SelectMany(qd => qd.AnswerDetails)
			.ToList()
			.ForEach(a => a.UserAnswer.IsReadOnly = true);
	}

	private void CalculateAllQuestions()
	{
		foreach (var analizedQuestion in AnalisedQuestions)
		{
			analizedQuestion.Score = _userAnswerEvaluator.CalculatePrice(analizedQuestion.Question);
			if (analizedQuestion.Score == analizedQuestion.Question.EvaluationPrice)
			{
				analizedQuestion.AnalizedResult = AnalizedQuestionResult.Correct;
				continue;
			}
			if (analizedQuestion.Score <= 0.0f)
			{
				analizedQuestion.AnalizedResult = AnalizedQuestionResult.Incorrect;
				continue;
			}
			analizedQuestion.AnalizedResult = AnalizedQuestionResult.PartiallyCorrect;
		}

		CountCorrectAnswers = AnalisedQuestions.Count(q => q.AnalizedResult == AnalizedQuestionResult.Correct);
		CountPartiallyCorrectAnswers = AnalisedQuestions.Count(q => q.AnalizedResult == AnalizedQuestionResult.PartiallyCorrect);
		CountIncorrectAnswers = AnalisedQuestions.Count(q => q.AnalizedResult == AnalizedQuestionResult.Incorrect);

		OnPropertyChanged(nameof(SumCurrentScore));
		OnPropertyChanged(nameof(SumMaxScore));
	}

	private void RetryPractice(object? parameter)
	{
		var confirmationViewModel = new ConfirmationWindowViewModel(
			"Are you sure?", 
			"Retry this quiz?");
		var result = _confirmationNavigationService.Navigate(confirmationViewModel);

		if (result)
		{
			_backNavigationService.Navigate();
		}
	}
}
