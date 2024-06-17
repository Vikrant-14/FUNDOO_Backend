using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<NoteLabelEntity> NoteLabelEntities { get; set; }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NoteLabelEntity>()
                .HasKey(nl => new { nl.NoteId, nl.LabelId });

            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabel)
                .HasForeignKey(nl => nl.NoteId);

            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabel)
                .HasForeignKey(nl => nl.LabelId);
        }

    }
}
