namespace QuizGenerator.Model.Entities;

public class AnswerDetail : Entity
{
    public override Guid Id { get; set; }
    public Guid? QuestionDetailId { get; set; }
    public QuestionDetail? QuestionDetail { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public AnswerDetail()
        : this(null)
    {
    }

    public AnswerDetail(QuestionDetail? questionDetail, string? text = null, bool isCorrect = false)
        : this(Guid.NewGuid(), questionDetail, text ?? string.Empty, isCorrect)
    { }

    public AnswerDetail(Guid id, QuestionDetail? questionDetail, string text, bool isCorrect)
    {
        Id = id;
        QuestionDetailId = questionDetail?.Id;
        QuestionDetail = questionDetail;
        Text = text;
        IsCorrect = isCorrect;
    }

	public override int GetHashCode()
	{
		return (Id, QuestionDetailId, Text, IsCorrect).GetHashCode();
	}
}
