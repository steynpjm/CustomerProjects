using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SteynPJM.CustomerProjects.DatabaseLibrary
{
    public partial class BaseDataContext : DbContext
    {
        public BaseDataContext()
        {
        }

        public BaseDataContext(DbContextOptions<BaseDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("COMPANY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(200)
                    .HasColumnName("address2");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.DeletedIndicator)
                    .IsRequired()
                    .HasColumnName("deletedIndicator")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("postalCode");

                entity.Property(e => e.Town)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("town");

                entity.Property(e => e.Version)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("PROJECT");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("code");

                entity.Property(e => e.CompanyHid).HasColumnName("companyHID");

                entity.Property(e => e.DeletedIndicator)
                    .IsRequired()
                    .HasColumnName("deletedIndicator")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.ManagerHid).HasColumnName("managerHID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Version)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("version");

                entity.HasOne(d => d.CompanyH)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.CompanyHid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECT_COMPANY");

                entity.HasOne(d => d.ManagerH)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ManagerHid)
                    .HasConstraintName("FK_PROJECT_USER_manager");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.HasIndex(e => e.Username, "UI_DBO_USER_userName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyHid).HasColumnName("companyHID");

                entity.Property(e => e.DeletedIndicator)
                    .IsRequired()
                    .HasColumnName("deletedIndicator")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Designation)
                    .HasMaxLength(150)
                    .HasColumnName("designation");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("lastname");

                entity.Property(e => e.Password)
                    .HasMaxLength(40)
                    .HasColumnName("password");

                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .HasColumnName("title");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.Version)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("version");

                entity.HasOne(d => d.CompanyH)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.CompanyHid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_COMPANY");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
