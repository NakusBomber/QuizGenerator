using QuizGenerator.ViewModel.Other;
using QuizGenerator.ViewModel.ViewModels.Bases;


namespace QuizGenerator.ViewModel.ViewModels.Models;


public class AnalisysQuestionViewModel : ViewModelBase
{
	private float _score;

	public float Score
	{
		get => _score;
		set
		{
			_score = value;
			OnPropertyChanged();
		}
	}


	private AnalizedQuestionResult _analizedResult;

	public AnalizedQuestionResult AnalizedResult
	{
		get => _analizedResult;
		set
		{
			_analizedResult = value;
			OnPropertyChanged();
		}
	}


	private QuestionViewModel _questionViewModel;

	public QuestionViewModel Question
	{
		get => _questionViewModel;
		set
		{
			_questionViewModel = value;
			OnPropertyChanged();
		}
	}


	public AnalisysQuestionViewModel(QuestionViewModel questionViewModel)
	{
		_questionViewModel = questionViewModel;
		_analizedResult = AnalizedQuestionResult.Correct;
	}
}
