﻿using QuizGenerator.Model.Entities;
using System.Linq.Expressions;

namespace QuizGenerator.Model.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
	public Task<IEnumerable<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
		bool asNoTracking = false,
		CancellationToken token = default);
	public Task<TEntity> GetByIdAsync(
		Guid id,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
		CancellationToken token = default);
	public Task CreateAsync(TEntity entity, CancellationToken token = default);
	public Task UpdateAsync(TEntity entity, CancellationToken token = default);
	public Task UpdateOrCreateAsync(TEntity entity, CancellationToken token = default);
	public Task DeleteAsync(TEntity entity, CancellationToken token = default);
}
