using System;
using System.Collections.Generic;

#nullable disable

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
    public partial class Company
    {
        public Company()
        {
            Project = new HashSet<Project>();
            User = new HashSet<User>();
        }

        public long Id { get; set; }
        public bool? DeletedIndicator { get; set; }
        public byte[] Version { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
