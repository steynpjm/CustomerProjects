using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
  public partial class BaseDataContext
  {
    public BaseDataContext(string connectionString)
    {
      _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(_connectionString);
      }

      base.OnConfiguring(optionsBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {

    }

    private readonly string _connectionString;

  }
}
