﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;

namespace SIO.Migrations.Migrations.OpenEventSourcing.Store
{
    [DbContext(typeof(OpenEventSourcingDbContext))]
    [Migration("20200102074142_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OpenEventSourcing.EntityFrameworkCore.Entities.Command", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("AggregateId");

                    b.Property<Guid>("CorrelationId");

                    b.Property<string>("Data");

                    b.Property<Guid>("Id");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("SequenceNo");

                    b.HasIndex("AggregateId");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Name");

                    b.ToTable("Command","log");
                });

            modelBuilder.Entity("OpenEventSourcing.EntityFrameworkCore.Entities.Event", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("AggregateId");

                    b.Property<Guid>("CausationId");

                    b.Property<Guid>("CorrelationId");

                    b.Property<string>("Data");

                    b.Property<Guid>("Id");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("SequenceNo");

                    b.HasIndex("AggregateId");

                    b.HasIndex("CausationId");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Name");

                    b.ToTable("Event","log");
                });

            modelBuilder.Entity("OpenEventSourcing.EntityFrameworkCore.Entities.Query", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("CorrelationId");

                    b.Property<string>("Data");

                    b.Property<Guid>("Id");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("SequenceNo");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Name");

                    b.ToTable("Query","log");
                });
#pragma warning restore 612, 618
        }
    }
}
