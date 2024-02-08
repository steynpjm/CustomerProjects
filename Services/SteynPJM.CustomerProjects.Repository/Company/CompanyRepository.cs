using Microsoft.EntityFrameworkCore;
using SteynPJM.CustomerProjects.DatabaseLibrary;


namespace SteynPJM.CustomerProjects.Repository.Company
{
  public class CompanyRepository : RepositoryBase<DatabaseLibrary.Company>
  {

    public async Task<DatabaseLibrary.Company?> GetByCompanyName(string companyName)
    {
			if (string.IsNullOrEmpty(companyName)) return null;

			var result = _dbSet.AsQueryable()
										.Where(x => x.Name.ToLower() == companyName.ToLower());

			return await result.FirstOrDefaultAsync();
    }


		public CompanyRepository(BaseDataContext dbContext) : base(dbContext)
		{
		}

	}
}
