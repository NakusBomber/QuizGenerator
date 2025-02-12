namespace QuizGenerator.Model.Entities;

public class Question : Entity
{
    public override Guid Id { get; set; }
    public Guid? QuizId { get; set; }
    public Quiz? Quiz { get; set; }
    public int EvaluationPrice { get; set; }
    public int ListNumber { get; set; }
    public QuestionType QuestionType { get; set; }

    public IEnumerable<QuestionDetail> QuestionDetails { get; set; }

    public Question()
        : this(null, 1, QuestionType.OneMore, 0)
    {
    }

    public Question(
        Quiz? quiz,
        int price,
        QuestionType questionType,
        int listNumber,
        IEnumerable<QuestionDetail>? questionDetails = null)
        : this(Guid.NewGuid(),
                quiz,
                price,
                questionType,
                listNumber,
                questionDetails ?? new List<QuestionDetail>())
    { }

    public Question(
        Guid id,
        Quiz? quiz,
        int price,
        QuestionType questionType,
        int listNumber,
        IEnumerable<QuestionDetail> questionDetails)
    {
        Id = id;
        QuizId = quiz?.Id;
        Quiz = quiz;
        EvaluationPrice = price;
        QuestionType = questionType;
        ListNumber = listNumber;
        QuestionDetails = questionDetails;
    }

	public override int GetHashCode()
	{
		return (Id, QuizId, EvaluationPrice, QuestionType, ListNumber).GetHashCode();
	}
}
