
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("UserAnswers")]
public class UserAnswer : Entity
{
	public override Guid Id { get; set; }

	public string? Text { get; set; }

	public Guid? PracticeSessionId { get; set; }
	public virtual PracticeSession? PracticeSession { get; set; }

	public Guid? QuestionDetailId { get; set; }
	public virtual QuestionDetail? QuestionDetail { get; set; }

	public Guid? AnswerDetailId { get; set; }
	public virtual AnswerDetail? AnswerDetail { get; set; }

	public UserAnswer()
		: this(Guid.NewGuid())
	{
	}

	public UserAnswer(
		Guid id, 
		string? text = null, 
		Guid? practiceSessionId = null, 
		Guid? questionDetailId = null,
		Guid? answerDetailId = null)
	{
		Id = id;
		Text = text;
		PracticeSessionId = practiceSessionId;
		QuestionDetailId = questionDetailId;
		AnswerDetailId = answerDetailId;
	}

	public override int GetHashCode()
	{
		return (Id, Text, AnswerDetailId, QuestionDetailId).GetHashCode();
	}
}
