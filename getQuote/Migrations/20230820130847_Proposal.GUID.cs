using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class ProposalGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GUID",
                table: "Proposal",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_GUID",
                table: "Proposal",
                column: "GUID",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Proposal_GUID", table: "Proposal");

            migrationBuilder.DropColumn(name: "GUID", table: "Proposal");
        }
    }
}
