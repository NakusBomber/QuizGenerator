using Microsoft.EntityFrameworkCore;
using QuizGenerator.Model.Entities;
using QuizGenerator.Model.Interfaces;
using System.Linq.Expressions;

namespace QuizGenerator.DAL;

public class GeneralRepository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	private readonly ApplicationContext _context;
	private readonly DbSet<TEntity> _dbSet;

	public GeneralRepository(ApplicationContext context)
	{
		_context = context;
		_dbSet = _context.Set<TEntity>();
	}

	public async Task CreateAsync(TEntity entity, CancellationToken token = default) =>
		await _dbSet.AddAsync(entity, token);

	public Task DeleteAsync(TEntity entity, CancellationToken token = default)
	{
		_dbSet.Remove(entity);
		return Task.CompletedTask;
	}

	public async Task<IEnumerable<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		bool asNoTracking = false,
		int? limit = null,
		CancellationToken token = default)
	{
		IQueryable<TEntity> query = _dbSet;

		if (filter != null)
		{
			query = query.Where(filter);
		}

		if (asNoTracking)
		{
			query = query.AsNoTracking();
		}

		if (orderBy != null)
		{
			query = orderBy(query);
		}

		if (limit.HasValue)
		{
			query = query.Take(limit.Value);
		}

		return await query.ToListAsync(token);
	}

	public async Task<TEntity> GetByIdAsync(
		Guid id,
		CancellationToken token = default)
	{
		var entity = (await GetAsync(e => e.Id == id, token: token)).FirstOrDefault();
		if (entity == null)
		{
			throw new InvalidOperationException($"Entity with id: {id} not found");
		}

		return entity;
	}

	public Task UpdateAsync(TEntity entity, CancellationToken token = default)
	{
		_dbSet.Update(entity);
		return Task.CompletedTask;
	}

	public async Task UpdateOrCreateAsync(TEntity entity, CancellationToken token = default)
	{
		var existingEntity = await _dbSet.FindAsync(new object?[] { entity.Id }, token);

		if (existingEntity == null)
		{
			await CreateAsync(entity, token);
		}
		else
		{
			_context.Entry(existingEntity).State = EntityState.Detached;
			await UpdateAsync(entity, token);
		}
	}
}
