namespace QuizGenerator.Model.Entities;

public class Question
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public int EvaluationPrice { get; set; }
    public QuestionType QuestionType { get; set; }

    public List<QuestionDetail> QuestionDetail { get; set; }
    public List<AnswerDetail> AnswerDetails { get; set; }


    public Question(
        Quiz quiz,
        int price,
        QuestionType questionType,
        List<QuestionDetail>? questionDetails = null,
        List<AnswerDetail>? answerDetails = null)
        : this(Guid.NewGuid(),
                quiz,
                price,
                questionType,
                questionDetails ?? new List<QuestionDetail>(),
                answerDetails ?? new List<AnswerDetail>())
    { }

    public Question(
        Guid id,
        Quiz quiz,
        int price,
        QuestionType questionType,
        List<QuestionDetail> questionDetails,
        List<AnswerDetail> answerDetails)
    {
        Id = id;
        QuizId = quiz.Id;
        Quiz = quiz;
        EvaluationPrice = price;
        QuestionType = questionType;
        QuestionDetail = questionDetails;
        AnswerDetails = answerDetails;
    }
}
