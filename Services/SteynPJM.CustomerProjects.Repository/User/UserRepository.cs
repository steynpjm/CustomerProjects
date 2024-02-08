using Microsoft.EntityFrameworkCore;

using SteynPJM.CustomerProjects.DatabaseLibrary;

namespace SteynPJM.CustomerProjects.Repository.User
{
  public class UserRepository : RepositoryBase<DatabaseLibrary.User>
  {
    public async Task<DatabaseLibrary.User?> GetByUserName(string userName)
    {
      var query = _dbSet.AsQueryable()
        .Where(x => x.Username.ToLower() == userName.ToLower());

      return await query.FirstOrDefaultAsync();
    }

    public async IAsyncEnumerable<DatabaseLibrary.User> GetAllForCompany(long companyHid, bool includeDeleted = false)
    {
      var query = _dbSet.AsQueryable()
        .Where(x => x.CompanyHid == companyHid);

      if (includeDeleted == false) query = query.Where(x => x.DeletedIndicator == false);

      await foreach (var user in query.AsAsyncEnumerable())
      {
        yield return user;
      }
    }

    public UserRepository(BaseDataContext dbContext) : base(dbContext)
    {
    }
  }
}
