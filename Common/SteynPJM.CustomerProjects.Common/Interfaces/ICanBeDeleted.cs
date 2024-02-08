using System;
using System.Collections.Generic;
using System.Text;

namespace SteynPJM.CustomerProjects.Common.Interfaces
{
  public interface ICanBeDeleted
  {
    bool? DeletedIndicator { get; set; }
  }
}
