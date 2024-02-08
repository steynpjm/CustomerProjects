using SteynPJM.CustomerProjects.DatabaseLibrary;

namespace SteynPJM.CustomerProjects.WebApi.Models
{
  public class LoginResultDto
  {
    public string Token { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Title { get; }
    public string Designation { get; }


    public LoginResultDto(User user, string token)
    {
      FirstName = user.Firstname;
      LastName = user.Lastname;
      Title = user.Title;
      Designation = user.Designation;
      Token = token;
    }
  }
}
