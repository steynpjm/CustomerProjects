using Microsoft.EntityFrameworkCore;

using SteynPJM.CustomerProjects.Common.Interfaces;
using SteynPJM.CustomerProjects.DatabaseLibrary;

namespace SteynPJM.CustomerProjects.Repository
{
	public class RepositoryBase<E> where E : class, IEntity, new()
	{
		protected readonly BaseDataContext _dbContext;
		protected readonly DbSet<E> _dbSet;

		/// <summary>
		/// Returns all entities from the database.
		/// </summary>
		/// <param name="includeDeleted">Indicate if deleted records must be returned as well.</param>
		/// <returns>A list of entities.</returns>
		public IAsyncEnumerable<E> GetAll(bool includeDeleted = false)
		{
			if (_dbContext is null) throw new NullReferenceException(nameof(_dbContext));

			IQueryable<E> data = _dbSet.AsQueryable<E>(); ;

			E? entity = new();
			if (entity is ICanBeDeleted && includeDeleted == false)
			{
				data = data.Where(x => ((ICanBeDeleted)x).DeletedIndicator == false);
			}

			return data.AsAsyncEnumerable<E>();
		}


		/// <summary>
		/// Get an entity by its Id from the database.
		/// </summary>
		/// <param name="id">The id of the entity to get.</param>
		/// <returns>The found entity or null.</returns>
		public async Task<E?> GetById(long id)
		{
			if (_dbContext is null) throw new NullReferenceException(nameof(_dbContext));

			E? entity = await _dbContext.FindAsync<E>(id);

			return entity;
		}

		/// <summary>
		/// Inserts a new entity into the database.
		/// </summary>
		/// <param name="entity">The entity to be inserted.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		public async Task<E> Insert(E entity)
		{
			if (_dbContext is null) throw new NullReferenceException(nameof(_dbContext));

			if (entity.Id != 0) throw new ArgumentOutOfRangeException(nameof(entity.Id));  // A new entity will always have an id of zero (0).

			await _dbContext.AddAsync<E>(entity);

			_dbContext.SaveChanges();

			await _dbContext.Entry(entity).ReloadAsync();

			return entity;
		}

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">The updated entity.</param>
		/// <returns>The updated entity.</returns>
		public async Task<E> Update(E entity)
		{
			if (_dbContext is null) throw new NullReferenceException(nameof(_dbContext));

			if (entity is null) throw new ArgumentNullException(nameof(entity));
			if (entity.Id <= 0) throw new ArgumentOutOfRangeException(nameof(entity));    // The id must be greater than zero (0).

			E? currentEntity = await _dbContext.FindAsync<E>(entity.Id);
			if (currentEntity is null) throw new Exception("Current record not found.");

			// If this entity checks concurrency, check version of current entity against passed in entity.
			ICheckConcurrency? concurrent = entity as ICheckConcurrency;
			if (concurrent is not null)
			{
				byte[] versionDB = (byte[])_dbContext.Entry(entity).Property(nameof(concurrent.Version)).OriginalValue;

				if (!versionDB.SequenceEqual(concurrent.Version)) throw new Exception("Concurrency Exception");
			}

			_dbContext.Update(entity);

			_dbContext.SaveChanges();

			await _dbContext.Entry(entity).ReloadAsync();

			return entity;
		}

		/// <summary>
		/// Deletes the entity.
		/// NOTE: If entity has the ICanBeDeleted interface on it, it's deleted indicator must updated instead.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		/// <returns>The update entity (ICanBeDeleted) or null if hard deleted.</returns>
		public async Task<E?> Delete(E entity)
		{
			E? result = null;

			if (entity is null) throw new ArgumentNullException(nameof(entity));
			if (entity.Id <= 0) throw new ArgumentOutOfRangeException(nameof(entity));    // The id must be greater than zero (0).

			if (_dbContext is null) throw new NullReferenceException(nameof(_dbContext));

			// If this entity checks concurrency, check version of current entity against passed in entity.
			ICheckConcurrency? concurrent = entity as ICheckConcurrency;
			if (concurrent is not null)
			{
				byte[] versionDB = (byte[])_dbContext.Entry(entity).Property(nameof(concurrent.Version)).OriginalValue;

				if (!versionDB.SequenceEqual(concurrent.Version)) throw new Exception("Concurrency Exception");
			}

			// If an entity is marked as ICanBeDeleted, then set the deleted indicator instead of doing a hard delete.
			ICanBeDeleted? canBeDeletedEntity = entity as ICanBeDeleted;
			if (canBeDeletedEntity is not null)
			{
				// Do update. Set deletedIndicator to true.
				canBeDeletedEntity.DeletedIndicator = true;
				_dbContext.Update(entity);

				_dbContext.SaveChanges();

				await _dbContext.Entry(entity).ReloadAsync();

				result = entity;
			}
			else
			{
				// do hard delete.
				_dbContext.Remove(entity);

				_dbContext.SaveChanges();
			}

			return result;
		}

		public RepositoryBase(BaseDataContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<E>();
		}
	}
}
