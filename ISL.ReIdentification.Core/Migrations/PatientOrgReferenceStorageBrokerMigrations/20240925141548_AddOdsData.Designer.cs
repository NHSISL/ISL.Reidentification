﻿// <auto-generated />
using System;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ISL.ReIdentification.Core.Migrations.PatientOrgReferenceStorageBrokerMigrations
{
    [DbContext(typeof(PatientOrgReferenceStorageBroker))]
    [Migration("20240925141548_AddOdsData")]
    partial class AddOdsData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ISL.ReIdentification.Core.Models.Foundations.OdsDatas.OdsData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Depth")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationCode_Parent")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("OrganisationCode_Root")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("OrganisationPrimaryRole_Parent")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("OrganisationPrimaryRole_Root")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTimeOffset>("RelationshipEndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("RelationshipStartDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("OdsDatas");
                });

            modelBuilder.Entity("ISL.ReIdentification.Core.Models.Foundations.PdsDatas.PdsData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("PdsDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
