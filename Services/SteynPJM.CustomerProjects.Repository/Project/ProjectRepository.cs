using Microsoft.EntityFrameworkCore;

using SteynPJM.CustomerProjects.DatabaseLibrary;

namespace SteynPJM.CustomerProjects.Repository.Project
{
  public class ProjectRepository : RepositoryBase<DatabaseLibrary.Project>
  {

		public async Task<DatabaseLibrary.Project?> GetByName(string ProjectName)
		{
			if (string.IsNullOrEmpty(ProjectName)) return null;

			var result = _dbSet.AsQueryable()
									.Where(x => x.Name.ToLower() == ProjectName.ToLower());

			return await result.FirstOrDefaultAsync();
		}

		public async Task<DatabaseLibrary.Project?> GetByCode(string code)
    {
      var query = _dbSet.AsQueryable()
									.Where(x => x.Code == code);

      return await query.FirstOrDefaultAsync();
    }

		public ProjectRepository(BaseDataContext dbContext) : base(dbContext)
    {
    }
  }
}
