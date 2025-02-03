using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;

namespace QuizGenerator.Model.Models.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
	private readonly TimeSpan _saveDelay;

	public IRepository<Quiz> QuizRepository { get; }
	public IRepository<Question> QuestionRepository { get; }
	public IRepository<QuestionDetail> QuestionDetailRepository { get; }
	public IRepository<AnswerDetail> AnswerDetailRepository { get; }

	public UnitOfWorkFake(TimeSpan saveDelay, TimeSpan delay)
		: this(saveDelay, 
			  new RepositoryFake<Quiz>(delay), 
			  new RepositoryFake<Question>(delay),
			  new RepositoryFake<QuestionDetail>(delay),
			  new RepositoryFake<AnswerDetail>(delay))
	{
	}

	public UnitOfWorkFake(
		TimeSpan saveDelay,
		IRepository<Quiz> quizRepository,
		IRepository<Question> questionRepository,
		IRepository<QuestionDetail> questionDetailRepository,
		IRepository<AnswerDetail> answerDetailRepository)
	{
		_saveDelay = saveDelay;

		QuizRepository = quizRepository;
		QuestionRepository = questionRepository;
		QuestionDetailRepository = questionDetailRepository;
		AnswerDetailRepository = answerDetailRepository;
	}

	public async Task SaveAsync()
	{
		await Task.Delay(_saveDelay);
	}
}
