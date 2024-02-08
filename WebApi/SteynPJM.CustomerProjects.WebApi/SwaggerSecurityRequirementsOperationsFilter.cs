using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace SteynPJM.CustomerProjects.WebApi
{
  public class SwaggerSecurityRequirementsOperationsFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      IList<object> metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
      if (metadata.Where(x => x is AuthorizeAttribute).Any())
      {
        IList<OpenApiSecurityRequirement> list = BuildApiKeyRequirement();

        operation.Security = list;
      }
    }

    private static IList<OpenApiSecurityRequirement> BuildApiKeyRequirement()
    {
      OpenApiSecurityScheme schema = new()
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header,
      };

      OpenApiSecurityRequirement requirement = new()
      {
        { schema, new List<string>() }
      };

      IList<OpenApiSecurityRequirement> list = new List<OpenApiSecurityRequirement>
      {
        requirement
      };

      return list;
    }
  }
}
