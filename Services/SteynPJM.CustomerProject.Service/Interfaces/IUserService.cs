using SteynPJM.CustomerProjects.Common.Models;

namespace SteynPJM.CustomerProjects.Service.Interfaces
{
  public interface IUserService : IServiceBase<DatabaseLibrary.User>
  {
    IAsyncEnumerable<DatabaseLibrary.User> GetAllForCompany(long companyHid, bool includeDeleted = false);
    Task<DatabaseLibrary.User?> GetById(long id);
    Task<DatabaseLibrary.User?> GetByUserName(string userName);
    Task<DatabaseLibrary.User> ResetPassword(DatabaseLibrary.User userEntity);
    Task<string?> GenerateToken(string userName, string password);
	}
}
