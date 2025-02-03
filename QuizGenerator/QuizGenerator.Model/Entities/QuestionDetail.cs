namespace QuizGenerator.Model.Entities;

public class QuestionDetail : Entity
{
    public override Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public string Text { get; set; }


    public QuestionDetail(Question question, string? text = null)
        : this(Guid.NewGuid(), question, text ?? string.Empty)
    {
    }

    public QuestionDetail(Guid id, Question question, string text)
    {
        Id = id;
        QuestionId = question.Id;
        Question = question;
        Text = text;
    }

	public override int GetHashCode()
	{
		return (Id, QuestionId, Text).GetHashCode();
	}
}
