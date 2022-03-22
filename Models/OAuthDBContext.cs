using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OAuthApp.Models
{
    public partial class OAuthDBContext : DbContext
    {
        public OAuthDBContext()
        {
        }

        public OAuthDBContext(DbContextOptions<OAuthDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<AuthUserProfile> AuthUserProfile { get; set; }
        public virtual DbSet<AuthUserRole> AuthUserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=AKURWIN;Database=OAuthDB;uid=bootcamp;pwd=pass123");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Islogged).HasColumnName("islogged");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuthUserProfile>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuthUserRole>(entity =>
            {
                entity.HasKey(e => e.Roleid);

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Rolename)
                    .IsRequired()
                    .HasColumnName("rolename")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
