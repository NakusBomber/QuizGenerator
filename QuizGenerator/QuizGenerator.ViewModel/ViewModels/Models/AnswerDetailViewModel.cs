using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class AnswerDetailViewModel : ViewModelBase
{
	private Guid _id;

	public Guid Id => _id;

	private string _text;

	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			OnPropertyChanged();
		}
	}

	private bool _isCorrect;

	public bool IsCorrect
	{
		get => _isCorrect;
		set
		{
			_isCorrect = value;
			OnPropertyChanged();
		}
	}

	private UserAnswerViewModel _userAnswer;

	public UserAnswerViewModel UserAnswer
	{
		get => _userAnswer;
		set
		{
			_userAnswer = value;
			OnPropertyChanged();
		}
	}


	public AnswerDetailViewModel()
		: this(new AnswerDetail())
	{
	}
	
	public AnswerDetailViewModel(AnswerDetail answerDetail)
	{
		_id = answerDetail.Id;
		_text = answerDetail.Text;
		_isCorrect = answerDetail.IsCorrect;

		_userAnswer = new UserAnswerViewModel();
	}

	public void CopyToAnswerDetail(AnswerDetail answerDetail)
	{
		ArgumentNullException.ThrowIfNull(answerDetail);

		answerDetail.Text = _text;
		answerDetail.IsCorrect = _isCorrect;
	}
}
