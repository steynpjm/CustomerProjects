using Microsoft.AspNetCore.Mvc;
using SteynPJM.CustomerProjects.Common.Models;
using SteynPJM.CustomerProjects.Service.Interfaces;
using SteynPJM.CustomerProjects.WebApi.Models;

namespace SteynPJM.CustomerProjects.WebApi
{
	public static class UsersEndpointsExtension
	{
		public static WebApplication MapUsersEndpoints(this WebApplication app)
		{

			// Login. No need to be authorized;
			app.MapPost("system/login", async (LoginDto loginData, IUserService userService) =>
			{
				var user = await userService.GetByUserName(loginData.UserName);

				if (user is null) return Results.Unauthorized();
				if (user.DeletedIndicator == true) return Results.Unauthorized();

				var token = await userService.GenerateToken(loginData.UserName, loginData.Password);

				if (token is null) return Results.Unauthorized();

				LoginResultDto result = new(user, token);

				return Results.Ok(result);
			})
			.AllowAnonymous()
			.WithTags("Login")
			.WithName("SystemLogin");


			// Users.
			app.MapGet("/user/list", async (IUserService userService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				var users = await userService.GetAllForCompany(loggedInCompanyHid.Value, false).ToListAsync();

				return Results.Ok(users);

			})
			.RequireAuthorization()
			.WithTags("Users")
			.WithName("GetAllUsers");


			// Get a specific user.
			app.MapGet("/user/{id}", async (long id, IUserService userService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				var user = await userService.GetById(id);

				if (user is null) return Results.NotFound();

				if (user.CompanyHid != loggedInCompanyHid.Value) return Results.NotFound();   // Only return users that belong to the same user as the logged in user.
																																											// TODO: Also handle case were user is a super user. A super user should be able to see all users.

				return Results.Ok(user);

			})
			.RequireAuthorization()
			.WithTags("Users")
			.WithName("GetUserById");


			//Post a new user.
			app.MapPost("/user", async (NewUserDto dtoData, IUserService userService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				try
				{
					DatabaseLibrary.User? user = new()
					{
						Username = dtoData.Username,
						Title = dtoData.Title,
						Firstname = dtoData.Firstname,
						Lastname = dtoData.Lastname,
						Designation = dtoData.Designation,
						Email = dtoData.Email,

						Password = _defaultPassword,
						CompanyHid = loggedInCompanyHid.Value,      // Create user for the same company as the logged in user.
					};

					user = await userService.Insert(user);

					if (user is not null)
					{
						return Results.Ok(user);
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
			.WithTags("Users")
			.WithName("AddNewUser");


			// Update a current user.
			app.MapPut("/user/{id}", async (long id, [FromBody] DatabaseLibrary.User userData, IUserService userService, HttpContext httpContext) =>
			{
				try
				{
					long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
					if (loggedInCompanyHid is null) return Results.BadRequest();

					if (id <= 0) return Results.BadRequest(nameof(id));

					var user = await userService.GetById(id);

					if (user is null) return Results.NotFound();
					if (user.CompanyHid != loggedInCompanyHid.Value) return Results.BadRequest("Bad Company"); // Can only update users in the same company as the logged in user.

					user.Title = userData.Title;
					user.Firstname = userData.Firstname;
					user.Lastname = userData.Lastname;
					user.Email = userData.Email;
					user.Designation = userData.Designation;

					user = await userService.Update(user);

					return Results.Ok(user);
				}
				catch (Exception ex)
				{
					return Results.BadRequest(ex);
				}
			})
			.RequireAuthorization()
			.WithTags("Users")
			.WithName("UpdateCurrentUser");


			// Delete a user.
			app.MapDelete("/user/{id}", async (long id, IUserService userService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				if (id <= 0) return Results.BadRequest(nameof(id));

				var user = await userService.GetById(id);

				if (user is null) return Results.NotFound();

				if (user.CompanyHid != loggedInCompanyHid.Value) return Results.BadRequest("Bad Company"); // Can only delete users in the same company as the logged in user.

				user = await userService.Delete(user);

				if (user is not null) return Results.Ok(user);        // If this was a soft delete, the updated user details are returned.
				return Results.Ok();                                  // If it was a hard delete, then no user exist to be returned.

			})
			.RequireAuthorization()
			.WithTags("Users")
			.WithName("DeleteUserById");


			// A list of users for dropdown display.
			app.MapGet("/user/lookup", async (IUserService userService, HttpContext httpContext) =>
			{
				long? loggedInCompanyHid = CustomerProjectExtension.GetLoggedInCompanyHid(httpContext.User.Identity);
				if (loggedInCompanyHid is null) return Results.BadRequest();

				List<LookupListDto> list = new List<LookupListDto>();

				await foreach(var value in userService.GetLookupValues())
				{
					list.Add(new LookupListDto() { Id = value.Id, Value = value.Value });
				}

				return Results.Ok(list);
				
			})
			.RequireAuthorization()
			.WithTags("Users")
			.WithName("UserLookupList");



			return app;
		}

		private const string _defaultPassword = "change me";
	}
}
