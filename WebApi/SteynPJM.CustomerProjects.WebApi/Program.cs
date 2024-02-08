using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using SteynPJM.CustomerProjects.DatabaseLibrary;
using SteynPJM.CustomerProjects.Service.User;
using SteynPJM.CustomerProjects.WebApi.Models;
using Microsoft.Extensions.Configuration;
using SteynPJM.CustomerProjects.Common.Models;
using System.Runtime.CompilerServices;

namespace SteynPJM.CustomerProjects.WebApi
{
  public class Program
  {
    private const string CorsPolicyName = "CORSDefault";

    public static void Main(string[] args)
    {

      var builder = WebApplication.CreateBuilder(args);


      JwtOptions tokenOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();


      builder.Services.AddSingleton(tokenOptions);

      builder.Services.AddDbContext<BaseDataContext>(options =>
        options
        .EnableSensitiveDataLogging()
        .UseSqlServer(builder.Configuration.GetConnectionString("PlayDatabase")));


      // Add services to the container.
      builder.Services.AddAuthorization();
      builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
         {
           options.TokenValidationParameters = new TokenValidationParameters()
           {
             LifetimeValidator = TokenLifetimeValidation,
             RequireExpirationTime = true,
             ValidateAudience = true,
             ValidateLifetime = false,
             ValidateActor = false,
             ValidateTokenReplay = false,
             ValidateIssuerSigningKey = true,
             ValidateIssuer = true,
             ValidIssuer = tokenOptions.Issuer,
             ValidAudience = tokenOptions.Audience,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.SigningKey)),
             ClockSkew = TimeSpan.MaxValue
           };
         });

      builder.Services.AddCors(options =>
      {
        options.AddPolicy(CorsPolicyName, builder =>
        {
          builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
      });

      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen(
        options =>
        {
          OpenApiInfo info = new OpenApiInfo { Title = "Customer Projects", Version = "v1", Description = "Exposes the Web API endpoints." };

          options.SwaggerDoc("v1", info);

          options.OperationFilter<SwaggerSecurityRequirementsOperationsFilter>();

          options.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
              Description = "Api Key needed to access the endpoints.",
              In = ParameterLocation.Header,
              Name = "Authorization",
              Type = SecuritySchemeType.ApiKey,
              BearerFormat = "JWT",
              Scheme = "Bearer"
            });
        });
      
      builder.Services.AddModulesForCustomerProjects(builder.Configuration);



      WebApplication app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
      }

      //app.UseHttpsRedirection();

      app.UseCors(CorsPolicyName);


      app.UseAuthentication();
      app.UseAuthorization();

      app.MapUsersEndpoints();
      app.MapCompaniesEndpoints();
      app.MapProjectsEndpoints();

      app.Run();
    }

    private static bool TokenLifetimeValidation(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
      bool result = false;
      DateTime dateTime = DateTime.UtcNow;

      if (notBefore <= dateTime && expires >= dateTime) result = true;

      return result;
    }

  }
}
