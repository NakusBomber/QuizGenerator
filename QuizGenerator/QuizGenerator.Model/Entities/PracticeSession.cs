using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("PracticeSessions")]
public class PracticeSession : Entity
{
	public override Guid Id { get; set; }
	
	public Guid? QuizId { get; set; }
	public virtual Quiz? Quiz { get; set; }

	public virtual IEnumerable<UserAnswer> UserAnswers { get; set; }

	public PracticeSession()
		: this(Guid.NewGuid())
	{
	}

	public PracticeSession(
		Guid id, 
		Guid? quizId = null,
		IEnumerable<UserAnswer>? userAnswers = null)
	{
		Id = id;
		QuizId = quizId;
		UserAnswers = userAnswers ?? new List<UserAnswer>();
	}

	public override int GetHashCode()
	{
		return (Id, QuizId).GetHashCode();
	}
}
