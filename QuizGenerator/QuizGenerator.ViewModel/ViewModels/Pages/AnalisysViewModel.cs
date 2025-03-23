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

		_pieChartData = new ObservableCollection<PieChartData>([
				new PieChartData("1", 1),
				new PieChartData("2", 2)]);



		RetryCommand = new DelegateCommand(RetryPractice);

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
