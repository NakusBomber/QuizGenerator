using QuizGenerator.Model.Entities;

namespace QuizGenerator.Model.Interfaces;

public interface IUnitOfWork
{
	public IRepository<Quiz> QuizRepository { get; }
	public IRepository<Question> QuestionRepository { get; }
	public IRepository<QuestionDetail> QuestionDetailRepository { get; }
	public IRepository<AnswerDetail> AnswerDetailRepository { get; }

	public Task SaveAsync(CancellationToken token);
}
