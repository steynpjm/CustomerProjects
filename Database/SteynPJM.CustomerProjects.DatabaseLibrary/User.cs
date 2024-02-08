using System;
using System.Collections.Generic;

#nullable disable

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
    public partial class User
    {
        public User()
        {
            Project = new HashSet<Project>();
        }

        public long Id { get; set; }
        public bool? DeletedIndicator { get; set; }
        public byte[] Version { get; set; }
        public long CompanyHid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }

        public virtual Company CompanyH { get; set; }
        public virtual ICollection<Project> Project { get; set; }
    }
}
