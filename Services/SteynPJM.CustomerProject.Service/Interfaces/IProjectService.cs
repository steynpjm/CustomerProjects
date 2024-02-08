



namespace SteynPJM.CustomerProjects.Service.Interfaces
{
	public interface IProjectService : IServiceBase<DatabaseLibrary.Project>
	{
		Task<DatabaseLibrary.Project?> GetByProjectCode(string projectCode);
		Task<DatabaseLibrary.Project?> GetByProjectName(string projectName);
	}
}
