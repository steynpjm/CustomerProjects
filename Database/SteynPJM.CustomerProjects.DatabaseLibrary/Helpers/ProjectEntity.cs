using SteynPJM.CustomerProjects.Common.Interfaces;

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
  public partial class Project : IEntity, ITrackUpdate, ICanBeDeleted, ICheckConcurrency
  {
  }
}
