﻿// <auto-generated />
using System;
using DbContextLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbContextLib.Migrations.SqlServerMigrations
{
    [DbContext(typeof(SqlServerDbContext))]
    partial class SqlServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DbModelsLib.csAlbum", b =>
                {
                    b.Property<Guid>("AlbumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CopiesSold")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("int");

                    b.Property<Guid?>("csMusicGroupMusicGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AlbumId");

                    b.HasIndex("csMusicGroupMusicGroupId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("DbModelsLib.csArtist", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("csMusicGroupMusicGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArtistId");

                    b.HasIndex("csMusicGroupMusicGroupId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("DbModelsLib.csMusicGroup", b =>
                {
                    b.Property<Guid>("MusicGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstablishedYear")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("MusicGroupId");

                    b.ToTable("MusicGroups");
                });

            modelBuilder.Entity("DbModelsLib.dtoAlbum", b =>
                {
                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("CopiesSold")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("int");

                    b.HasKey("AlbumId");

                    b.ToTable((string)null);

                    b.ToView("vwAlbums", "usr");
                });

            modelBuilder.Entity("DbModelsLib.dtoArtist", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("ArtistId");

                    b.ToTable((string)null);

                    b.ToView("vwArtists", "usr");
                });

            modelBuilder.Entity("DbModelsLib.dtoMusicGroup", b =>
                {
                    b.Property<Guid>("MusicGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstablishedYear")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("MusicGroupId");

                    b.ToTable((string)null);

                    b.ToView("vwMusicGroups", "usr");
                });

            modelBuilder.Entity("DbModelsLib.csAlbum", b =>
                {
                    b.HasOne("DbModelsLib.csMusicGroup", null)
                        .WithMany("Albums")
                        .HasForeignKey("csMusicGroupMusicGroupId");
                });

            modelBuilder.Entity("DbModelsLib.csArtist", b =>
                {
                    b.HasOne("DbModelsLib.csMusicGroup", null)
                        .WithMany("Members")
                        .HasForeignKey("csMusicGroupMusicGroupId");
                });

            modelBuilder.Entity("DbModelsLib.csMusicGroup", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}