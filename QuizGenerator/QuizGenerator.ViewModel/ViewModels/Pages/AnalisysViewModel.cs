using QuizGenerator.View.IndependentComponents.Models;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Other.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using QuizGenerator.ViewModel.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace QuizGenerator.ViewModel.ViewModels.Pages;

public class AnalisysViewModel : ViewModelBase
{
	private readonly IBackNavigationService _backNavigationService;
	private readonly IWindowNavigationService<ConfirmationWindowViewModel, bool> _confirmationNavigationService;

	private TrainingViewModel _trainingSession;

	public TrainingViewModel TrainingSession
	{
		get => _trainingSession;
		set
		{
			_trainingSession = value;
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
		_trainingSession = trainingViewModel;
		_confirmationNavigationService = confirmationNavigationService;
		_backNavigationService = backNavigationService;

		SetIsReadOnly();

		_pieChartData = new ObservableCollection<PieChartData>([
				new PieChartData("Correct", 0, Brushes.Green),
				new PieChartData("Incorrect", 0, Brushes.Red)]);

		CountCorrectAnswers = 5;
		CountIncorrectAnswers = 3;

		RetryCommand = new DelegateCommand(RetryPractice);
	}

	private void OnScoreChanged()
	{
		PieChartData.ElementAt(0).Weight = CountCorrectAnswers;
		PieChartData.ElementAt(1).Weight = CountIncorrectAnswers;
	}

	private void SetIsReadOnly()
	{
		var questions = TrainingSession.Quiz?.Questions;
		if (questions == null || questions.Count == 0)
		{
			return;
		}

		questions
			.SelectMany(q => q.QuestionDetails)
			.SelectMany(qd => qd.AnswerDetails)
			.ToList()
			.ForEach(a => a.UserAnswer.IsReadOnly = true);
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
