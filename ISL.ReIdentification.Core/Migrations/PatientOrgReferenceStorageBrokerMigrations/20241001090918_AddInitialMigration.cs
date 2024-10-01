using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISL.ReIdentification.Core.Migrations.PatientOrgReferenceStorageBrokerMigrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdsDatas",
                columns: table => new
                {
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PseudoNhsNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PrimaryCareProvider = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PrimaryCareProviderBusinessEffectiveFromDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PrimaryCareProviderBusinessEffectiveToDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CcgOfRegistration = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CurrentCcgOfRegistration = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IcbOfRegistration = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CurrentIcbOfRegistration = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsDatas", x => x.RowId);
                    table.UniqueConstraint("AK_PdsDatas_CcgOfRegistration", x => x.CcgOfRegistration);
                    table.UniqueConstraint("AK_PdsDatas_CurrentCcgOfRegistration", x => x.CurrentCcgOfRegistration);
                    table.UniqueConstraint("AK_PdsDatas_CurrentIcbOfRegistration", x => x.CurrentIcbOfRegistration);
                    table.UniqueConstraint("AK_PdsDatas_IcbOfRegistration", x => x.IcbOfRegistration);
                });

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
                    table.ForeignKey(
                        name: "FK_OdsDatas_PdsDatas_OrganisationCode_Root",
                        column: x => x.OrganisationCode_Root,
                        principalTable: "PdsDatas",
                        principalColumn: "CurrentIcbOfRegistration",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdsDatas_OrganisationCode_Root",
                table: "OdsDatas",
                column: "OrganisationCode_Root");
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
