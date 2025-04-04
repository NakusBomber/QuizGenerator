using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("Questions")]
public class Question : Entity
{
    public override Guid Id { get; set; }

    public Guid? QuizId { get; set; }
    public virtual Quiz? Quiz { get; set; }

    public int EvaluationPrice { get; set; }
    public int ListNumber { get; set; }
    public QuestionType QuestionType { get; set; }

    public virtual IEnumerable<QuestionDetail> QuestionDetails { get; set; }

    public Question()
        : this(null, 1, QuestionType.One, 0)
    {
    }

    public Question(
        Guid? quizId,
        int price,
        QuestionType questionType,
        int listNumber,
        IEnumerable<QuestionDetail>? questionDetails = null)
        : this(Guid.NewGuid(),
                quizId,
                price,
                questionType,
                listNumber,
                questionDetails ?? new List<QuestionDetail>())
    { }

    public Question(
        Guid id,
        Guid? quizId,
        int price,
        QuestionType questionType,
        int listNumber,
        IEnumerable<QuestionDetail> questionDetails)
    {
        Id = id;
        QuizId = quizId;
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
