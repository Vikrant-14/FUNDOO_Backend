﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryLayer.Context;

#nullable disable

namespace RepositoryLayer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240617081348_CollaboratorConstraints")]
    partial class CollaboratorConstraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RepositoryLayer.Entity.CollaboratorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CollaboratorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NoteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("CollaboratorEmail", "NoteId")
                        .IsUnique();

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("LabelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTrashed")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.NoteLabelEntity", b =>
                {
                    b.Property<int>("NoteId")
                        .HasColumnType("int");

                    b.Property<int>("LabelId")
                        .HasColumnType("int");

                    b.HasKey("NoteId", "LabelId");

                    b.HasIndex("LabelId");

                    b.ToTable("NoteLabelEntities");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.CollaboratorEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entity.Note", "Note")
                        .WithMany("Collaborators")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.NoteLabelEntity", b =>
                {
                    b.HasOne("RepositoryLayer.Entity.Label", "Label")
                        .WithMany("NoteLabel")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryLayer.Entity.Note", "Note")
                        .WithMany("NoteLabel")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");

                    b.Navigation("Note");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.Label", b =>
                {
                    b.Navigation("NoteLabel");
                });

            modelBuilder.Entity("RepositoryLayer.Entity.Note", b =>
                {
                    b.Navigation("Collaborators");

                    b.Navigation("NoteLabel");
                });
#pragma warning restore 612, 618
        }
    }
}
