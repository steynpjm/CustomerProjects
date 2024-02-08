using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using SteynPJM.CustomerProjects.Repository.Company;
using SteynPJM.CustomerProjects.Repository.Project;
using SteynPJM.CustomerProjects.Repository.User;
using SteynPJM.CustomerProjects.Service.Company;
using SteynPJM.CustomerProjects.Service.Interfaces;
using SteynPJM.CustomerProjects.Service.Project;
using SteynPJM.CustomerProjects.Service.User;

namespace SteynPJM.CustomerProjects.WebApi
{
  public static class CustomerProjectExtension
  {

    public static IServiceCollection AddModulesForCustomerProjects(this IServiceCollection services, ConfigurationManager configuration)
    {

      services.AddScoped<UserRepository>();
      services.AddScoped<CompanyRepository>();
      services.AddScoped<ProjectRepository>();

      services.AddScoped<IUserService, UserService>();
      services.AddScoped<ICompanyService, CompanyService>();
      services.AddScoped<IProjectService, ProjectService>();

      return services;
    }

    public static long? GetLoggedInCompanyHid(System.Security.Principal.IIdentity? loggedInuser)
    {
      if (loggedInuser is null) return null;

      if (loggedInuser is not ClaimsIdentity identity) return null;

      var claims = identity.Claims;
      var companyHidClaim = claims.Where(c => c.Type == "companyHid").Select(c => c.Value).SingleOrDefault();
      if (long.TryParse(companyHidClaim, out var companyHid) == false)
      {
        return null;
      }

      return companyHid;
    }

  }
}
