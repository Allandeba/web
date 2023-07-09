using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class refactoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Contact_Person_PersonId", table: "Contact");

            migrationBuilder.DropForeignKey(name: "FK_Document_Person_PersonId", table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

            migrationBuilder.DropIndex(
                name: "IX_ProposalContent_ItemId1",
                table: "ProposalContent"
            );

            migrationBuilder.DropIndex(name: "IX_Proposal_PersonId1", table: "Proposal");

            migrationBuilder.DropColumn(name: "ItemId1", table: "ProposalContent");

            migrationBuilder.DropColumn(name: "PersonId1", table: "Proposal");

            migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Proposal",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                column: "PersonId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Person_PersonId",
                table: "Contact",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_PersonId",
                table: "Document",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId",
                table: "Proposal",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Contact_Person_PersonId", table: "Contact");

            migrationBuilder.DropForeignKey(name: "FK_Document_Person_PersonId", table: "Document");

            migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

            migrationBuilder.DropIndex(name: "IX_ProposalContent_ItemId", table: "ProposalContent");

            migrationBuilder.DropIndex(name: "IX_Proposal_PersonId", table: "Proposal");

            migrationBuilder.DropColumn(name: "ItemId", table: "ProposalContent");

            migrationBuilder.DropColumn(name: "PersonId", table: "Proposal");

            migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "ProposalContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            migrationBuilder.AddColumn<int>(
                name: "ItemId1",
                table: "ProposalContent",
                type: "int",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "Proposal",
                type: "int",
                nullable: true
            );

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Document",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId1",
                table: "ProposalContent",
                column: "ItemId1"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId1",
                table: "Proposal",
                column: "PersonId1"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Person_PersonId",
                table: "Contact",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_PersonId",
                table: "Document",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent",
                column: "ItemId1",
                principalTable: "Item",
                principalColumn: "ItemId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "ProposalId"
            );
        }
    }
}
