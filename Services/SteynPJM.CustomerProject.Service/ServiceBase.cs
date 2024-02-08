using SteynPJM.CustomerProjects.Common.Interfaces;
using SteynPJM.CustomerProjects.Common.Models;
using SteynPJM.CustomerProjects.Repository;
using SteynPJM.CustomerProjects.Service.Interfaces;

namespace SteynPJM.CustomerProjects.Service
{
	public abstract class ServiceBase<E> : IServiceBase<E> where E : class, IEntity, new()
	{

		public IAsyncEnumerable<E> GetAll(bool includeDeleted = false)
		{
			return _repository.GetAll();
		}

		public Task<E?> GetById(long id)
		{
			throw new NotImplementedException();
		}

		public async Task<E?> Insert(E entity)
		{
			if (entity is null) return null;

			await ValidateInsert(entity);

			await _repository.Insert(entity);

			return entity;
		}

		public async Task<E?> Update(E entity)
		{
			if (entity is null) return null;

			E? originalEntity = await _repository.GetById(entity.Id);

			if (originalEntity is null) throw new Exception("Original entity not found.");

			await ValidateUpdate(entity, originalEntity);
			await _repository.Update(entity);

			return entity;
		}

		public async Task<E?> Delete(E entity)
		{
			if (entity is null) return null;

			E? originalEntity = await _repository.GetById(entity.Id);

			if (originalEntity is null) throw new Exception("Original entity not found.");

			await ValidateDelete(originalEntity);
			E? result = await _repository.Delete(entity);

			return result;
		}

		public async IAsyncEnumerable<LookupListValue> GetLookupValues()
		{
			IAsyncEnumerable<E> data = GetAll(false);

			await foreach (E value in data)
			{
				yield return ConvertToLookupValue(value);
			}
		}

		public ServiceBase(RepositoryBase<E> repository)
		{
			_repository = repository;
		}

		protected readonly RepositoryBase<E> _repository;

		protected abstract Task ValidateInsert(E entity);
		protected abstract Task ValidateUpdate(E entity, E originalEntity);
		protected abstract Task ValidateDelete(E entity);

		protected abstract LookupListValue ConvertToLookupValue(E value);	


	}
}
