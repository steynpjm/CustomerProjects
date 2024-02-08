using SteynPJM.CustomerProjects.Common.Models;
using SteynPJM.CustomerProjects.Repository.Project;
using SteynPJM.CustomerProjects.Service.Interfaces;

namespace SteynPJM.CustomerProjects.Service.Project
{
	public class ProjectService : ServiceBase<DatabaseLibrary.Project>, IProjectService
	{

		/// <summary>
		/// Get a list of all the projects.
		/// </summary>
		/// <param name="includeDeleted">Indicate if deleted projects must be included.</param>
		/// <returns>A list of projects.</returns>
		public IAsyncEnumerable<DatabaseLibrary.Project> GetAll(bool includeDeleted = false)
		{
			ProjectRepository repository = (ProjectRepository)_repository;

			return repository.GetAll(includeDeleted);
		}

		/// <summary>
		/// Get a Project by its id.
		/// </summary>
		/// <param name="id">The id of the Project to get.</param>
		/// <returns>The Project for the specified id if it exists, otherwise null.</returns>
		public Task<DatabaseLibrary.Project?> GetById(long id)
		{
			return _repository.GetById(id);
		}

		/// <summary>
		/// Get a Project by its Projectname.
		/// </summary>
		/// <param name="projectName">The Name of the Project to find.</param>
		/// <returns>The Project for the specified Projectname if it exists, otherwise null.</returns>
		public Task<DatabaseLibrary.Project?> GetByProjectName(string projectName)
		{
			ProjectRepository repository = (ProjectRepository)_repository;
			return repository.GetByName(projectName);
		}

		/// <summary>
		/// Get a Project by its Projectcode.
		/// </summary>
		/// <param name="projectCode">The Code of the Project to find.</param>
		/// <returns>The Project for the specified Projectcode if it exists, otherwise null.</returns>
		public Task<DatabaseLibrary.Project?> GetByProjectCode(string projectCode)
		{
			ProjectRepository repository = (ProjectRepository)_repository;
			return repository.GetByCode(projectCode);
		}


		public ProjectService(ProjectRepository repository) : base(repository)
		{
		}

		protected override Task ValidateDelete(DatabaseLibrary.Project entity)
		{
			return Task.CompletedTask;
		}

		protected override async Task ValidateInsert(DatabaseLibrary.Project entity)
		{
			// There can only be one entry with the same Project name.
			string projectName = entity.Name;
			DatabaseLibrary.Project? otherProjectWithProjectName = await GetByProjectName(projectName);
			if (otherProjectWithProjectName is not null) throw new InvalidDataException("Another Project with that name already exist.");

			// There can only be on entry with the same Project code.
			var projectCode = entity.Code;
			DatabaseLibrary.Project? otherProjectWithProjectCode = await GetByProjectCode(projectCode);
			if (otherProjectWithProjectName is not null) throw new InvalidDataException("Another Project with that code already exist.");


		}

		protected override Task ValidateUpdate(DatabaseLibrary.Project entity, DatabaseLibrary.Project originalEntity)
		{
			return Task.CompletedTask;
		}

		protected override LookupListValue ConvertToLookupValue(DatabaseLibrary.Project value)
		{
			return new()
			{
				Id = value.Id,
				Value = value.Name
			};
		}
	}
}
