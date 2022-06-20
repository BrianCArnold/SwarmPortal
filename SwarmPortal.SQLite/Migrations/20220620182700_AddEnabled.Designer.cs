﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwarmPortal.SQLite;

#nullable disable

namespace SwarmPortal.SQLite.Migrations
{
    [DbContext(typeof(SourceContext))]
    [Migration("20220620182700_AddEnabled")]
    partial class AddEnabled
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("LinkRole", b =>
                {
                    b.Property<ulong>("LinksId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("RolesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LinksId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("LinkRole");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.Group", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.Link", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("SwarmPortalUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SwarmPortalUserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.Role", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.SwarmPortalUser", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("OIDCUserKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LinkRole", b =>
                {
                    b.HasOne("SwarmPortal.SQLite.Link", null)
                        .WithMany()
                        .HasForeignKey("LinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwarmPortal.SQLite.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SwarmPortal.SQLite.Link", b =>
                {
                    b.HasOne("SwarmPortal.SQLite.Group", "Group")
                        .WithMany("Links")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwarmPortal.SQLite.SwarmPortalUser", null)
                        .WithMany("Links")
                        .HasForeignKey("SwarmPortalUserId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.Group", b =>
                {
                    b.Navigation("Links");
                });

            modelBuilder.Entity("SwarmPortal.SQLite.SwarmPortalUser", b =>
                {
                    b.Navigation("Links");
                });
#pragma warning restore 612, 618
        }
    }
}
