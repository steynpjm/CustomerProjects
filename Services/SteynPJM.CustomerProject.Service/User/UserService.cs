using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using SteynPJM.CustomerProjects.Common.Models;
using SteynPJM.CustomerProjects.Repository.User;
using SteynPJM.CustomerProjects.Service.Interfaces;

namespace SteynPJM.CustomerProjects.Service.User
{
  public class UserService : ServiceBase<DatabaseLibrary.User>, IUserService
  {
    /// <summary>
    /// Get all users for the specified company.
    /// </summary>
    /// <param name="companyHid">The id of the company to get users for.</param>
    /// <param name="includeDeleted">Should users marked as deleted be included.</param>
    /// <returns>A list of users</returns>
    public IAsyncEnumerable<DatabaseLibrary.User> GetAllForCompany(long companyHid, bool includeDeleted = false)
    {
      if (companyHid <= 0) throw new ArgumentOutOfRangeException(nameof(companyHid));

      UserRepository repository = (UserRepository)_repository;

      return repository.GetAllForCompany(companyHid, includeDeleted);
    }

    /// <summary>
    /// Get a user by its id.
    /// </summary>
    /// <param name="id">The id of the user to get.</param>
    /// <returns>The user for the specified id if it exists, otherwise null.</returns>
    public Task<DatabaseLibrary.User?> GetById(long id)
    {
      return _repository.GetById(id);
    }

    /// <summary>
    /// Get a user by its username.
    /// </summary>
    /// <param name="userName">The username of the user to find.</param>
    /// <returns>The user for the specified username if it exists, otherwise null.</returns>
    public Task<DatabaseLibrary.User?> GetByUserName(string userName)
    {
      UserRepository repository = (UserRepository)_repository;
      return repository.GetByUserName(userName);
    }

    /// <summary>
    /// Reset the password details of the user.
    /// NOTE: It basically sets the password to "change me".
    /// </summary>
    /// <param name="userEntity">The entity to reset the user for.</param>
    /// <returns>The updated user entity.</returns>
    public async Task<DatabaseLibrary.User> ResetPassword(DatabaseLibrary.User userEntity)
    {
      UserRepository repository = (UserRepository)_repository;

      if (userEntity is null) throw new ArgumentNullException(nameof(userEntity));
      if (userEntity.DeletedIndicator == true) throw new ArgumentException(nameof(userEntity));  // Cannot reset password for a deleted user.

      userEntity.Password = "change me";

      await repository.Update(userEntity);

      return userEntity;
    }

    /// <summary>
    /// Generate a token for the username and password.
    /// NOTE: It checks if the username exists and if the associated user is not deleted.
    ///       If all is well, a token is generated for this user.
    /// </summary>
    /// <returns>A JWT token, othwerwise null.</returns>
    public async Task<string?> GenerateToken(string userName, string password)
    {
      if (userName is null) return null;
      if (password is null) return null;

      try
      {
        // Check if user exist with username and if password match.
        UserRepository repository = (UserRepository)_repository;

        DatabaseLibrary.User? userEntity = await repository.GetByUserName(userName);
        if (userEntity is null) return null;
        if (userEntity.Password != password) return null;
        if (userEntity.DeletedIndicator == true) return null;

        string token = GenerateJwtToken(userEntity);

        if (token is null) return null;

        return token;
      }
      catch (Exception)
      {
        // Do not do anything. Just return silently.
        return null;
      }
    }

    public UserService(JwtOptions tokenOptions, UserRepository repository) : base(repository)
    {
      _tokenOptions = tokenOptions;
    }

    /// <summary>
    /// Validation that must be applied when deleting new user.
    /// </summary>
    /// <param name="entity">The entity to be checked.</param>
    protected override Task ValidateDelete(DatabaseLibrary.User entity)
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Validation that must be applied when inserting a new user.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override async Task ValidateInsert(DatabaseLibrary.User entity)
    {
      UserRepository repository = (UserRepository)_repository;

      // There can only be one entry with the same username.
      string userName = entity.Username;
      DatabaseLibrary.User? otherUserWithUserName = await repository.GetByUserName(userName);
      if (otherUserWithUserName is not null) throw new InvalidDataException("Another user with that username already exist.");

    }

    /// <summary>
    /// Validation that must be applied when updating a user.
    /// </summary>
    /// <param name="entity">The entity to be checked.</param>
    protected override Task ValidateUpdate(DatabaseLibrary.User entity, DatabaseLibrary.User originalEntity)
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Generates a JWT token with claims for the passed in user.
    /// </summary>
    /// <param name="user">The user to generate a JWT token for.</param>
    /// <returns></returns>
    private string GenerateJwtToken(DatabaseLibrary.User user)
    {
      JwtSecurityTokenHandler tokenHandler = new();
      byte[] key = Encoding.ASCII.GetBytes(_tokenOptions.SigningKey);

      List<Claim> claims = new()
      {
        new Claim("userHid", user.Id.ToString()),
        new Claim("companyHid", user.CompanyHid.ToString()),
      };

      SecurityTokenDescriptor tokenDescriptor = new()
      {
        Subject = new ClaimsIdentity(claims),
        NotBefore = DateTime.UtcNow,
        IssuedAt = DateTime.UtcNow,
        Expires = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpirationSeconds),
        Issuer = _tokenOptions.Issuer,
        Audience = _tokenOptions.Audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

		protected override LookupListValue ConvertToLookupValue(DatabaseLibrary.User value)
		{
			return new()
			{
				Id = value.Id,
				Value = $"{value.Firstname} {value.Lastname}"
			};
		}

		private readonly JwtOptions _tokenOptions;
  }
}
