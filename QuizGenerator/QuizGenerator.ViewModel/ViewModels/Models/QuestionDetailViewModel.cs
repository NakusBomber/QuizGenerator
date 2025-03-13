using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class QuestionDetailViewModel : ViewModelBase
{
	private Guid? _questionId;
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


	private ICollection<AnswerDetailViewModel> _answerDetails;

	public ICollection<AnswerDetailViewModel> AnswerDetails
	{
		get => _answerDetails;
		set
		{
			_answerDetails = value;
			OnPropertyChanged();
		}
	}

	public QuestionDetailViewModel()
		: this(new QuestionDetail())
	{
	}

	public QuestionDetailViewModel(QuestionDetail questionDetail)
	{
		_id = questionDetail.Id;
		_questionId = questionDetail.QuestionId;
		_text = questionDetail.Text;
		_answerDetails = new ObservableCollection<AnswerDetailViewModel>();
	}

	public void CopyToQuestionDetail(QuestionDetail questionDetail)
	{
		ArgumentNullException.ThrowIfNull(questionDetail);

		questionDetail.QuestionId = _questionId;
		questionDetail.Text = _text;
	}
}
