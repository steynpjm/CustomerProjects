using Microsoft.AspNetCore.Mvc;
using SteynPJM.CustomerProjects.Service.Interfaces;
using SteynPJM.CustomerProjects.WebApi.Models;
using System.Xml.Linq;

namespace SteynPJM.CustomerProjects.WebApi
{
  public static class ProjectsEndpointsExtension
  {
    public static WebApplication MapProjectsEndpoints(this WebApplication app)
    {

			// Get all projects.
			app.MapGet("/project/list", async (IProjectService projectService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				List<DatabaseLibrary.Project> projects = await projectService.GetAll(false).ToListAsync();

				return Results.Ok(projects);

			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("GetAllProjects");


			// Get a specific project.
			app.MapGet("/project/{id}", async (long id, IProjectService projectService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				DatabaseLibrary.Project? project = await projectService.GetById(id);

				if (project is null) return Results.NotFound();

				return Results.Ok(project);

			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("GetProjectById");


			//Post a new project.
			app.MapPost("/project", async (NewProjectDto dtoData, IProjectService projectService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				try
				{
					DatabaseLibrary.Project? project = new()
					{
						Name = dtoData.Name,
						Code = dtoData.Code,
						CompanyHid	= dtoData.CompanyHid,
						Description = dtoData.Description,
						ManagerHid = dtoData.ManagerHid,
					};

					project = await projectService.Insert(project);

					if (project is not null)
					{
						return Results.Ok(project);
					}
					else
					{
						return Results.BadRequest();
					}
				}

				catch (Exception ex)
				{
					return Results.BadRequest(ex);
				}
			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("AddNewProject");


			// Update a current company.
			app.MapPut("/project/{id}", async (long id, [FromBody] DatabaseLibrary.Project projectData, IProjectService projectService, HttpContext httpContext) =>
			{
				try
				{
					long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
					if (loggedInCompanyHid is null) return Results.BadRequest();

					if (id <= 0) return Results.BadRequest(nameof(id));

					DatabaseLibrary.Project? project = await projectService.GetById(id);

					if (project is null) return Results.NotFound();

					project.Name = projectData.Name;
					project.Code = projectData.Code;
					project.Description = projectData.Description;
					project.CompanyHid = projectData.CompanyHid;
					project.ManagerHid = projectData.ManagerHid;

					project = await projectService.Update(project);

					return Results.Ok(project);
				}
				catch (Exception ex)
				{
					return Results.BadRequest(ex);
				}
			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("UpdateCurrentProject");


			// Delete a project.
			app.MapDelete("/project/{id}", async (long id, IProjectService projectService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				DatabaseLibrary.Project? project = await projectService.GetById(id);

				if (project is null) return Results.NotFound();
				project = await projectService.Delete(project);

				if (project is not null) return Results.Ok(project);        // If this was a soft delete, the updated project details are returned.
				return Results.Ok();                                        // If it was a hard delete, then no project exist to be returned.

			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("DeleteProjectById");


			// A list of projects for dropdown display.
			app.MapGet("/project/lookup", async (IProjectService projectService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				List<LookupListDto> list = new List<LookupListDto>();

				await foreach (var value in projectService.GetLookupValues())
				{
					list.Add(new LookupListDto() { Id = value.Id, Value = value.Value });
				}

				return Results.Ok(list);

			})
			.RequireAuthorization()
			.WithTags("Projects")
			.WithName("ProjectLookupList");


			return app;
    }
  }
}
