using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISL.ReIdentification.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAccessAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessAudit",
                table: "AccessAudit");

            migrationBuilder.RenameTable(
                name: "AccessAudit",
                newName: "AccessAudits");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "AccessAudits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessAudits",
                table: "AccessAudits",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccessAudits_CreatedDate",
                table: "AccessAudits",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccessAudits_HasAccess",
                table: "AccessAudits",
                column: "HasAccess");

            migrationBuilder.CreateIndex(
                name: "IX_AccessAudits_PseudoIdentifier",
                table: "AccessAudits",
                column: "PseudoIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AccessAudits_UserEmail",
                table: "AccessAudits",
                column: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessAudits",
                table: "AccessAudits");

            migrationBuilder.DropIndex(
                name: "IX_AccessAudits_CreatedDate",
                table: "AccessAudits");

            migrationBuilder.DropIndex(
                name: "IX_AccessAudits_HasAccess",
                table: "AccessAudits");

            migrationBuilder.DropIndex(
                name: "IX_AccessAudits_PseudoIdentifier",
                table: "AccessAudits");

            migrationBuilder.DropIndex(
                name: "IX_AccessAudits_UserEmail",
                table: "AccessAudits");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "AccessAudits");

            migrationBuilder.RenameTable(
                name: "AccessAudits",
                newName: "AccessAudit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessAudit",
                table: "AccessAudit",
                column: "Id");
        }
    }
}
