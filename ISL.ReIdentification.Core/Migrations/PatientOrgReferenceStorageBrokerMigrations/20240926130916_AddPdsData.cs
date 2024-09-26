using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISL.ReIdentification.Core.Migrations.PatientOrgReferenceStorageBrokerMigrations
{
    /// <inheritdoc />
    public partial class AddPdsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PdsDatas",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PdsDatas");

            migrationBuilder.AddColumn<long>(
                name: "RowId",
                table: "PdsDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CcgOfRegistration",
                table: "PdsDatas",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentCcgOfRegistration",
                table: "PdsDatas",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentIcbOfRegistration",
                table: "PdsDatas",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IcbOfRegistration",
                table: "PdsDatas",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCareProvider",
                table: "PdsDatas",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PrimaryCareProviderBusinessEffectiveFromDate",
                table: "PdsDatas",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PrimaryCareProviderBusinessEffectiveToDate",
                table: "PdsDatas",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PseudoNhsNumber",
                table: "PdsDatas",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PdsDatas",
                table: "PdsDatas",
                column: "RowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PdsDatas",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "CcgOfRegistration",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "CurrentCcgOfRegistration",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "CurrentIcbOfRegistration",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "IcbOfRegistration",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "PrimaryCareProvider",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "PrimaryCareProviderBusinessEffectiveFromDate",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "PrimaryCareProviderBusinessEffectiveToDate",
                table: "PdsDatas");

            migrationBuilder.DropColumn(
                name: "PseudoNhsNumber",
                table: "PdsDatas");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PdsDatas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PdsDatas",
                table: "PdsDatas",
                column: "Id");
        }
    }
}
