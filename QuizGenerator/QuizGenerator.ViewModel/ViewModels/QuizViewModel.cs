using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuizViewModel : ViewModelBase
{

	private Guid _id;

	public Guid Id
	{
		get => _id;
		set
		{
			_id = value;
			OnPropertyChanged();
		}
	}


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

	private DateTime _dateTimeCreated;

	public DateTime DateTimeCreated
	{
		get => _dateTimeCreated;
		set
		{
			_dateTimeCreated = value;
			OnPropertyChanged();
		}
	}

	private DateTime _dateTimeChanged;

	public DateTime DateTimeChanged
	{
		get => _dateTimeChanged;
		set
		{
			_dateTimeChanged = value;
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
	
	public QuizViewModel()
		: this(new Quiz())
	{
	}

	public QuizViewModel(Quiz quiz)
	{
		_id = quiz.Id;
		_name = quiz.Name;
		_dateTimeCreated = quiz.DateTimeCreated;
		_dateTimeChanged = quiz.DateTimeChanged;
		_isNeedInterval= quiz.IntervalPractice != null;
		_interval = quiz.IntervalPractice;
		_questions = new ObservableCollection<Question>(quiz.Questions);
	}

	public static explicit operator Quiz(QuizViewModel quizViewModel)
	{
		return new Quiz(
			quizViewModel.Id,
			quizViewModel.Name,
			quizViewModel.DateTimeCreated,
			quizViewModel.DateTimeChanged,
			null,
			quizViewModel.Interval,
			quizViewModel.Questions);
	}
}
