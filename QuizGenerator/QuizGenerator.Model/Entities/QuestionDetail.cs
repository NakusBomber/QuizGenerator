using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("QuestionDetails")]
public class QuestionDetail : Entity
{
    public override Guid Id { get; set; }

    public Guid? QuestionId { get; set; }
	public virtual Question? Question { get; set; }

    public string Text { get; set; }
    public virtual IEnumerable<AnswerDetail> AnswerDetails { get; set; }

    public QuestionDetail()
        : this(null)
    {
    }
    
    public QuestionDetail(Guid? questionId, string? text = null)
        : this(Guid.NewGuid(), questionId, text ?? string.Empty)
    {
    }

    public QuestionDetail(Guid id, Guid? questionId, string text, IEnumerable<AnswerDetail>? answerDetails = null)
    {
        Id = id;
        QuestionId = questionId;
        Text = text;
        AnswerDetails = answerDetails ?? new List<AnswerDetail>();
    }

	public override int GetHashCode()
	{
		return (Id, QuestionId, Text).GetHashCode();
	}
}
