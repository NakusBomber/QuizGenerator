using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("AnswerDetails")]
public class AnswerDetail : Entity
{
    public override Guid Id { get; set; }

    public Guid? QuestionDetailId { get; set; }
    public virtual QuestionDetail? QuestionDetail { get; set; }

    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public AnswerDetail()
        : this(null)
    {
    }

    public AnswerDetail(Guid? questionDetailId, string? text = null, bool isCorrect = false)
        : this(Guid.NewGuid(), questionDetailId, text ?? string.Empty, isCorrect)
    { }

    public AnswerDetail(Guid id, Guid? questionDetailId, string text, bool isCorrect)
    {
        Id = id;
        QuestionDetailId = questionDetailId;
        Text = text;
        IsCorrect = isCorrect;
    }

	public override int GetHashCode()
	{
		return (Id, QuestionDetailId, Text, IsCorrect).GetHashCode();
	}
}
