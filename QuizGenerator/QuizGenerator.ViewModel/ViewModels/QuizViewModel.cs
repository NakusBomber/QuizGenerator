using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using QuizGenerator.ViewModel.Commands.Bases;
using QuizGenerator.ViewModel.Commands.Interfaces;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.ViewModels;

public class QuizViewModel : ViewModelBase
{
	private Quiz _quiz;

	public Quiz Quiz
	{
		get => _quiz;
		set
		{
			_quiz = value;
			OnPropertyChanged();
		}
	}

	public QuizViewModel(Quiz? quiz)
	{
		_quiz = quiz ?? new Quiz();
	}
}
