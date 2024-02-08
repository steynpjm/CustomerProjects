using Microsoft.AspNetCore.Mvc;
using SteynPJM.CustomerProjects.Service.Interfaces;
using SteynPJM.CustomerProjects.WebApi.Models;

namespace SteynPJM.CustomerProjects.WebApi
{
	public static class CompaniesEndpointsExtension
	{
		public static WebApplication MapCompaniesEndpoints(this WebApplication app)
		{

			// Get all companies.
			app.MapGet("/company/list", async (ICompanyService companyService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				List<DatabaseLibrary.Company> companies = await companyService.GetAllCompanies(false).ToListAsync();

				return Results.Ok(companies);

			})
			.RequireAuthorization()
			.WithTags("Companies")
			.WithName("GetAllCompanies");


			// Get a specific company.
			app.MapGet("/company/{id}", async (long id, ICompanyService companyService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				DatabaseLibrary.Company? company = await companyService.GetById(id);

				if (company is null) return Results.NotFound();

				return Results.Ok(company);

			})
			.RequireAuthorization()
			.WithTags("Companies")
			.WithName("GetCompanyById");


			//Post a new compay.
			app.MapPost("/company", async (NewCompanyDto dtoData, ICompanyService companyService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				try
				{
					DatabaseLibrary.Company? company = new()
					{
						Name = dtoData.Name,
						Address1 = dtoData.Address1,
						Address2 = dtoData.Address2,
						Town = dtoData.Town,
						PostalCode = dtoData.PostalCode,
						Country = dtoData.Country,
					};

					company = await companyService.Insert(company);

					if (company is not null)
					{
						return Results.Ok(company);
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
			.WithTags("Companies")
			.WithName("AddNewCompany");


			// Update a current copmany.
			app.MapPut("/company/{id}", async (long id, [FromBody] DatabaseLibrary.Company companyData, ICompanyService companyService, HttpContext httpContext) =>
			{
				try
				{
					long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
					if (loggedInCompanyHid is null) return Results.BadRequest();

					if (id <= 0) return Results.BadRequest(nameof(id));

					DatabaseLibrary.Company? company = await companyService.GetById(id);

					if (company is null) return Results.NotFound();

					company.Name = companyData.Name;
					company.Address1 = companyData.Address1;
					company.Address2 = companyData.Address2;
					company.Town = companyData.Town;
					company.PostalCode = companyData.PostalCode;
					company.Country = companyData.Country;

					company = await companyService.Update(company);

					return Results.Ok(company);
				}
				catch (Exception ex)
				{
					return Results.BadRequest(ex);
				}
			})
			.RequireAuthorization()
			.WithTags("Companies")
			.WithName("UpdateCurrentCompany");


			// Delete a company.
			app.MapDelete("/company/{id}", async (long id, ICompanyService companyService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				DatabaseLibrary.Company? company = await companyService.GetById(id);

				if (company is null) return Results.NotFound();
				company = await companyService.Delete(company);

				if (company is not null) return Results.Ok(company);        // If this was a soft delete, the updated company details are returned.
				return Results.Ok();                                        // If it was a hard delete, then no company exist to be returned.

			})
			.RequireAuthorization()
			.WithTags("Companies")
			.WithName("DeleteCompanyById");


			// A list of companies for dropdown display.
			app.MapGet("/company/lookup", async (ICompanyService companyService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				List<LookupListDto> list = new List<LookupListDto>();

				await foreach (var value in companyService.GetLookupValues())
				{
					list.Add(new LookupListDto() { Id = value.Id, Value = value.Value });
				}

				return Results.Ok(list);

			})
			.RequireAuthorization()
			.WithTags("Companies")
			.WithName("CompanyLookupList");


			return app;
		}

	}
}
