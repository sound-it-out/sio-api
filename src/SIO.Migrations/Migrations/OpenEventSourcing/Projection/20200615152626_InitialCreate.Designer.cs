﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;

namespace SIO.Migrations.Migrations.OpenEventSourcing.Projection
{
    [DbContext(typeof(OpenEventSourcingProjectionDbContext))]
    [Migration("20200615152626_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OpenEventSourcing.EntityFrameworkCore.Entities.ProjectionState", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<long>("Position");

                    b.HasKey("Name");

                    b.ToTable("ProjectionState");
                });

            modelBuilder.Entity("SIO.Domain.Projections.Document.Document", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("Data");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<Guid?>("TranslationId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("SIO.Domain.Projections.User.User", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("Data");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SIO.Domain.Projections.UserDocument.UserDocument", b =>
                {
                    b.Property<Guid>("DocumentId");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("Data");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<Guid>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("DocumentId");

                    b.ToTable("UserDocument");
                });
#pragma warning restore 612, 618
        }
    }
}
