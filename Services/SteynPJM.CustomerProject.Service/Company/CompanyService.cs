using SteynPJM.CustomerProjects.Common.Models;
using SteynPJM.CustomerProjects.Repository.Company;
using SteynPJM.CustomerProjects.Service.Interfaces;

namespace SteynPJM.CustomerProjects.Service.Company
{
  public class CompanyService : ServiceBase<DatabaseLibrary.Company>, ICompanyService
  {

    /// <summary>
    /// Get a list of all the companies.
    /// </summary>
    /// <param name="includeDeleted">Indicate if deleted companies must be included.</param>
    /// <returns>A list of companies.</returns>
    public IAsyncEnumerable<DatabaseLibrary.Company> GetAllCompanies(bool includeDeleted = false)
    {
      var repository = (CompanyRepository)_repository;

      return repository.GetAll(includeDeleted);
    }

    /// <summary>
    /// Get a Company by its id.
    /// </summary>
    /// <param name="id">The id of the Company to get.</param>
    /// <returns>The Company for the specified id if it exists, otherwise null.</returns>
    public Task<DatabaseLibrary.Company?> GetById(long id)
    {
      return _repository.GetById(id);
    }

    /// <summary>
    /// Get a Company by its Companyname.
    /// </summary>
    /// <param name="companyName">The Name of the Company to find.</param>
    /// <returns>The Company for the specified Companyname if it exists, otherwise null.</returns>
    public Task<DatabaseLibrary.Company?> GetByCompanyName(string companyName)
    {
      var repository = (CompanyRepository)_repository;
      return repository.GetByCompanyName(companyName);
    }



    public CompanyService(CompanyRepository repository) : base(repository)
    {
    }


    /// <summary>
    /// Validation that must be applied when deleting new Company.
    /// </summary>
    /// <param name="entity">The entity to be checked.</param>
    protected override Task ValidateDelete(DatabaseLibrary.Company entity)
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Validation that must be applied when inserting a new Company.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override async Task ValidateInsert(DatabaseLibrary.Company entity)
    {

      // There can only be one entry with the same Company name.
      var companyName = entity.Name;
      var otherCompanyWithCompanyName = await GetByCompanyName(companyName);
      if (otherCompanyWithCompanyName is not null) throw new InvalidDataException("Another Company with that name already exist.");

    }

    /// <summary>
    /// Validation that must be applied when updating a Company.
    /// </summary>
    /// <param name="entity">The entity to be checked.</param>
    protected override Task ValidateUpdate(DatabaseLibrary.Company entity, DatabaseLibrary.Company originalEntity)
    {
      return Task.CompletedTask;
    }

		protected override LookupListValue ConvertToLookupValue(DatabaseLibrary.Company value)
		{
			return new()
			{
				Id = value.Id,
				Value = value.Name
			};
		}
	}
}
