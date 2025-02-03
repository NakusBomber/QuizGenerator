using QuizGenerator.Model.Entities;
using System.Linq.Expressions;

namespace QuizGenerator.Model.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
	public Task<IEnumerable<TEntity>> GetAsync(
		Expression<Func<TEntity, bool>>? filter = null,
		Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>>? orderBy = null,
		bool asNoTracking = false);
	public Task<TEntity> GetByIdAsync(Guid id);
	public Task CreateAsync(TEntity entity);
	public Task UpdateAsync(TEntity entity);
	public Task DeleteAsync(TEntity entity);
}
