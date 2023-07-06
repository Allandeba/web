using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class dropColumnProposalContentPersonId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Proposal",
                newName: "PersonId1"
            );

            migrationBuilder.RenameIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                newName: "IX_Proposal_PersonId1"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "PersonId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal"
            );

            migrationBuilder.RenameColumn(
                name: "PersonId1",
                table: "Proposal",
                newName: "PersonId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_Proposal_PersonId1",
                table: "Proposal",
                newName: "IX_Proposal_PersonId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId",
                table: "Proposal",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );
        }
    }
}
