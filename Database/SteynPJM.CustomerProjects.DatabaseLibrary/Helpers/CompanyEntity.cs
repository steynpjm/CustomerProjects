using SteynPJM.CustomerProjects.Common.Interfaces;

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
  public partial class Company : IEntity, ITrackUpdate, ICanBeDeleted, ICheckConcurrency
  {
  }
}
