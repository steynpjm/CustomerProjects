namespace SteynPJM.CustomerProjects.WebApi.Models
{
	public class NewProjectDto
	{
		public long CompanyHid { get; set; }
		public long? ManagerHid { get; set; }
		public string Name { get; set; } = string.Empty;	
		public string Description { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;

	}
}
