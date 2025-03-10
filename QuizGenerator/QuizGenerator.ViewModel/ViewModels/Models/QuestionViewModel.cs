using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class QuestionViewModel : ViewModelBase
{
	private Quiz? _quiz;
	private Guid? _quizId;

	private Guid _id;

	public Guid Id => _id;

	private int _evaluationPrice;

	public int EvaluationPrice
	{
		get => _evaluationPrice;
		set
		{
			_evaluationPrice = value;
			OnPropertyChanged();
		}
	}

	private int _listNumber;

	public int ListNumber
	{
		get => _listNumber;
		set
		{
			_listNumber = value;
			OnPropertyChanged();
		}
	}

	private QuestionType _questionType;

	public QuestionType QuestionType
	{
		get => _questionType;
		set
		{
			_questionType = value;
			OnPropertyChanged();
		}
	}

	private ICollection<QuestionDetailViewModel> _questionDetails;

	public ICollection<QuestionDetailViewModel> QuestionDetails
	{
		get => _questionDetails;
		set
		{
			_questionDetails = value;
			OnPropertyChanged();
		}
	}

	public QuestionViewModel()
		: this(new Question())
	{
	}

	public QuestionViewModel(Question question)
	{
		_id = question.Id;
		_quizId = question.QuizId;
		_quiz = question.Quiz;
		_evaluationPrice = question.EvaluationPrice;
		_listNumber = question.ListNumber;
		_questionType = question.QuestionType;
		_questionDetails = new ObservableCollection<QuestionDetailViewModel>(
			question.QuestionDetails.Select(qd => new QuestionDetailViewModel(qd)));
	}

	public void CopyToQuestion(Question question)
	{
		ArgumentNullException.ThrowIfNull(question);

		question.QuizId = _quizId;
		question.EvaluationPrice = _evaluationPrice;
		question.QuestionType = _questionType;
		question.ListNumber = _listNumber;
	}
}
