namespace SteynPJM.CustomerProjects.WebApi.Models
{
	public class NewCompanyDto
	{
		public string Name { get;  set; } = string.Empty;
		public string Address1 { get;  set; } = string.Empty;
		public string Address2 { get;  set; } = string.Empty;
		public string Town { get;  set; } = string.Empty;
		public string PostalCode { get;  set; } = string.Empty;
		public string Country { get;  set; } = string.Empty;
	}
}
