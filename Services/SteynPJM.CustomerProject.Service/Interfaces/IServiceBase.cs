using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteynPJM.CustomerProjects.Common.Interfaces;
using SteynPJM.CustomerProjects.Common.Models;

namespace SteynPJM.CustomerProjects.Service.Interfaces
{
  public interface IServiceBase<E> where E : class, IEntity, new()
  {
		IAsyncEnumerable<E> GetAll(bool includeDeleted = false);
		Task<E?> GetById(long id);
		Task<E?> Delete(E entity);
    Task<E?> Insert(E entity);
    Task<E?> Update(E entity);
		IAsyncEnumerable<LookupListValue> GetLookupValues();
	}
}
