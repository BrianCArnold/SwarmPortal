﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwarmPortal.Context;

#nullable disable

namespace SwarmPortal.SqlServerMigrations.Migrations
{
    [DbContext(typeof(SourceContext))]
    partial class SourceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LinkRole", b =>
                {
                    b.Property<decimal>("LinksId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("RolesId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("LinksId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("LinkRole");
                });

            modelBuilder.Entity("SwarmPortal.Context.Group", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SwarmPortal.Context.Link", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<decimal>("GroupId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("SwarmPortalUserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SwarmPortalUserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("SwarmPortal.Context.Role", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SwarmPortal.Context.SwarmPortalUser", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("OIDCUserKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SwarmPortal.Context.UriIcon", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RetrievedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Uri")
                        .IsUnique();

                    b.ToTable("UriIcons");
                });

            modelBuilder.Entity("LinkRole", b =>
                {
                    b.HasOne("SwarmPortal.Context.Link", null)
                        .WithMany()
                        .HasForeignKey("LinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwarmPortal.Context.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SwarmPortal.Context.Link", b =>
                {
                    b.HasOne("SwarmPortal.Context.Group", "Group")
                        .WithMany("Links")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwarmPortal.Context.SwarmPortalUser", null)
                        .WithMany("Links")
                        .HasForeignKey("SwarmPortalUserId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SwarmPortal.Context.Group", b =>
                {
                    b.Navigation("Links");
                });

            modelBuilder.Entity("SwarmPortal.Context.SwarmPortalUser", b =>
                {
                    b.Navigation("Links");
                });
#pragma warning restore 612, 618
        }
    }
}
