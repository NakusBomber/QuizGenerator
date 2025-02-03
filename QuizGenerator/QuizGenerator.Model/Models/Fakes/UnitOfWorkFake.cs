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
	{
		_saveDelay = saveDelay;

		QuizRepository = new RepositoryFake<Quiz>(delay);
		QuestionRepository = new RepositoryFake<Question>(delay);
		QuestionDetailRepository = new RepositoryFake<QuestionDetail>(delay);
		AnswerDetailRepository = new RepositoryFake<AnswerDetail>(delay);
	}

	public async Task SaveAsync()
	{
		await Task.Delay(_saveDelay);
	}
}
