﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Web.Migrations
{
    [DbContext(typeof(PhotoFlickerContext))]
    [Migration("20201016085552_InitialMigration")]
    partial class InitialMigration
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Path = "https://planetescape.pl//app/uploads/2018/10/Wielki-Kanion-3.jpg"
                        },
                        new
                        {
                            Id = 2,
                            Path = "https://www.ef.pl/sitecore/__/~/media/universal/pg/8x5/destination/US_US-CA_LAX_1.jpg"
                        },
                        new
                        {
                            Id = 3,
                            Path = "https://i.wpimg.pl/1500x0/d.wpimg.pl/1036153311-705207955/zorza-polarna.jpg"
                        },
                        new
                        {
                            Id = 4,
                            Path = "https://www.banita.travel.pl/wp-content/uploads/2019/10/zorza-polarna-norwegia1-1920x1282.jpg"
                        });
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

                    b.HasKey("Id");

                    b.ToTable("TagItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Kanion"
                        },
                        new
                        {
                            Id = 2,
                            Name = "USA"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Zorza"
                        });
                });

            modelBuilder.Entity("PhotoTag", b =>
                {
                    b.Property<int>("MarkedPhotosId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("MarkedPhotosId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("PhotoTag");
                });

            modelBuilder.Entity("PhotoTag", b =>
                {
                    b.HasOne("PhotoFlicker.Models.Photo", null)
                        .WithMany()
                        .HasForeignKey("MarkedPhotosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PhotoFlicker.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
