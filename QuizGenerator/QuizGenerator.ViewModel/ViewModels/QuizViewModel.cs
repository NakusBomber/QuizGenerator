using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

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

	public ICommand OpenDropDownQuestionTypesCommand { get; }
	public ICommand AddNewQuestionCommand { get; }

	public QuizViewModel(Quiz? quiz)
	{
		_quiz = quiz ?? new Quiz();

		_name = _quiz.Name;
		_isNeedInterval = _quiz.IntervalPractice != null;
		_interval = _quiz.IntervalPractice;
		_questions = new ObservableCollection<Question>(_quiz.Questions);

		OpenDropDownQuestionTypesCommand = new DelegateCommand(OpenDropDownQuestionTypes);
		AddNewQuestionCommand = new DelegateCommand(AddNewQuestion);
	}

	private void AddNewQuestion(object? obj)
	{
		if (obj is QuestionType questionType)
		{
			var listNumber = (Questions.LastOrDefault()?.ListNumber + 1) ?? 0;
			var question = new Question(_quiz, 1, questionType, listNumber);
			Questions.Add(question);
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
}
