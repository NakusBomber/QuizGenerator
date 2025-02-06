using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using System.Linq.Expressions;

namespace QuizGenerator.Model.Models.Fakes;

public class RepositoryFake<TEntity> : IRepository<TEntity> where TEntity : Entity
{
	private readonly HashSet<TEntity> _set;
	private readonly TimeSpan _delay;

	public RepositoryFake(TimeSpan? delay = null, HashSet<TEntity>? entities = null) 
		: this(entities ?? new HashSet<TEntity>(), delay ?? TimeSpan.Zero)
	{
	}

	public RepositoryFake(HashSet<TEntity> entities, TimeSpan delay)
	{
		_set = entities;
		_delay = delay;
	}
	public async Task CreateAsync(TEntity entity, CancellationToken token = default)
	{
		await Task.Delay(_delay, token);
		_set.Add(entity);
	}

	public async Task DeleteAsync(TEntity entity, CancellationToken token = default)
	{
		await Task.Delay(_delay, token);
		_set.Remove(entity);
	}

	public async Task<IEnumerable<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null,
		bool asNoTracking = false,
		CancellationToken token = default)
	{
		await Task.Delay(_delay, token);

		IQueryable<TEntity> hashSet = _set.AsQueryable();

		if (filter != null)
		{
			hashSet = hashSet.Where(filter);
		}

		if (orderBy != null)
		{
			return orderBy(hashSet).ToList();
		}

		return hashSet.ToList();
	}

	public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default)
	{
		var entity = (await GetAsync(e => e.Id == id, token: token)).FirstOrDefault();
		if (entity == null)
		{
			throw new InvalidOperationException("Entity with this Id not found");
		}

		return entity;
	}

	public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
	{
		await Task.Delay(_delay, token);

		var entityRemove = _set.FirstOrDefault(e => e.Id == entity.Id);
		if (entityRemove != null)
		{
			_set.Remove(entityRemove);
			_set.Add(entity);
		}
		else
		{
			throw new ArgumentException("Entity not found");
		}
	}
}
