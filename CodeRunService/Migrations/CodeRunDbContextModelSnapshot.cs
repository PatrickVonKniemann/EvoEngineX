﻿// <auto-generated />
using System;
using CodeRunService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeRunService.Migrations
{
    [DbContext(typeof(CodeRunDbContext))]
    partial class CodeRunDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DomainEntities.CodeRun", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CodeBaseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ResultsId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("RunFinish")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RunStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResultsId");

                    b.ToTable("CodeRuns");
                });

            modelBuilder.Entity("Generics.Enums.RunResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.ToTable("RunResult");
                });

            modelBuilder.Entity("DomainEntities.CodeRun", b =>
                {
                    b.HasOne("Generics.Enums.RunResult", "Results")
                        .WithMany()
                        .HasForeignKey("ResultsId");

                    b.Navigation("Results");
                });
#pragma warning restore 612, 618
        }
    }
}
