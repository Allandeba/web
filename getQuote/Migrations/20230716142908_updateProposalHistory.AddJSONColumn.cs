using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class updateProposalHistoryAddJSONColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProposalContentArray",
                table: "ProposalHistory",
                newName: "ProposalContentJSON_ProposalContentItems"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProposalContentJSON_ProposalContentItems",
                table: "ProposalHistory",
                newName: "ProposalContentArray"
            );
        }
    }
}
