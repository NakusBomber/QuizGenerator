namespace QuizGenerator.Model.Entities;

public class AnswerDetail
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public AnswerDetail(Question question, string? text = null, bool isCorrect = false)
        : this(Guid.NewGuid(), question, text ?? string.Empty, isCorrect)
    { }

    public AnswerDetail(Guid id, Question question, string text, bool isCorrect)
    {
        Id = id;
        QuestionId = question.Id;
        Question = question;
        Text = text;
        IsCorrect = isCorrect;
    }
}
