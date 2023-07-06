using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class addColumnProposalContentQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Quantity", table: "ProposalContent");
        }
    }
}
