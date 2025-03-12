using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;

namespace QuizGenerator.DAL;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationContext _context;

	public IRepository<Quiz> QuizRepository { get; }
	public IRepository<Question> QuestionRepository { get; }
	public IRepository<QuestionDetail> QuestionDetailRepository { get; }
	public IRepository<AnswerDetail> AnswerDetailRepository { get; }

	public UnitOfWork(
		ApplicationContext context,
		IRepository<Quiz> quizRepository,
		IRepository<Question> questionRepository,
		IRepository<QuestionDetail> questionDetailRepository,
		IRepository<AnswerDetail> answerDetailRepository)
	{
		_context = context;

		QuizRepository = quizRepository;
		QuestionRepository = questionRepository;
		QuestionDetailRepository = questionDetailRepository;
		AnswerDetailRepository = answerDetailRepository;
	}

	public async Task SaveAsync(CancellationToken token)
	{
		await _context.SaveChangesAsync(token);
	}
}
	