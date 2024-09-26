using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISL.ReIdentification.Core.Migrations.PatientOrgReferenceStorageBrokerMigrations
{
    /// <inheritdoc />
    public partial class AddOdsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OdsDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationCode_Root = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrganisationPrimaryRole_Root = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    OrganisationCode_Parent = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrganisationPrimaryRole_Parent = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    RelationshipStartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RelationshipEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdsDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdsDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsDatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdsDatas");

            migrationBuilder.DropTable(
                name: "PdsDatas");
        }
    }
}
