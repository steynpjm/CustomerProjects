namespace SteynPJM.CustomerProjects.Service.Interfaces
{
  public interface ICompanyService : IServiceBase<DatabaseLibrary.Company>
  {
    IAsyncEnumerable<DatabaseLibrary.Company> GetAllCompanies(bool includeDeleted = false);
    Task<DatabaseLibrary.Company?> GetByCompanyName(string companyName);
    Task<DatabaseLibrary.Company?> GetById(long id);
  }
}
