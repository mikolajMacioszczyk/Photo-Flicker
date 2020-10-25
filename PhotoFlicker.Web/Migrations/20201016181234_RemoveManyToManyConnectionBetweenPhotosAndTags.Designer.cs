﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PhotoFlicker.Application.Context;

namespace PhotoFlicker.Web.Migrations
{
    [DbContext(typeof(PhotoFlickerContext))]
    [Migration("20201016181234_RemoveManyToManyConnectionBetweenPhotosAndTags")]
    partial class RemoveManyToManyConnectionBetweenPhotosAndTags
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0-rc.2.20475.6");

            modelBuilder.Entity("PhotoFlicker.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PhotoItems");
                });

            modelBuilder.Entity("PhotoFlicker.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("PhotoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("TagItems");
                });

            modelBuilder.Entity("PhotoFlicker.Models.Tag", b =>
                {
                    b.HasOne("PhotoFlicker.Models.Photo", null)
                        .WithMany("Tags")
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("PhotoFlicker.Models.Photo", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
