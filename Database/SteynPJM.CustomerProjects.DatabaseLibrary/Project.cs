using System;
using System.Collections.Generic;

#nullable disable

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
    public partial class Project
    {
        public long Id { get; set; }
        public bool? DeletedIndicator { get; set; }
        public byte[] Version { get; set; }
        public long CompanyHid { get; set; }
        public long? ManagerHid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public virtual Company CompanyH { get; set; }
        public virtual User ManagerH { get; set; }
    }
}
