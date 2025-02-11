using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuizViewModel : ViewModelBase
{
	private Quiz _quiz;

	private string _name;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}


	private bool _isNeedInterval;

	public bool IsNeedInterval
	{
		get => _isNeedInterval;
		set
		{
			_isNeedInterval = value;
			OnPropertyChanged();
		}
	}

	private TimeSpan? _interval;

	public TimeSpan? Interval
	{
		get => _interval;
		set
		{
			_interval = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<Question> _questions;

	public ObservableCollection<Question> Questions
	{
		get => _questions;
		set
		{
			_questions = value;
			OnPropertyChanged();
		}
	}

	public QuizViewModel(Quiz? quiz)
	{
		_quiz = quiz ?? new Quiz();

		_name = _quiz.Name;
		_isNeedInterval = _quiz.IntervalPractice != null;
		_interval = _quiz.IntervalPractice;
		_questions = new ObservableCollection<Question>(_quiz.Questions);
	}
}
