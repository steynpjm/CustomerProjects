using SteynPJM.CustomerProjects.Common.Interfaces;

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
  public partial class User : IEntity, ITrackUpdate, ICanBeDeleted, ICheckConcurrency
  {
  }
}
