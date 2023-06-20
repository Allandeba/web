using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class updatePrincipaltoMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "Principal", table: "ItemImage", newName: "Main");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "Main", table: "ItemImage", newName: "Principal");
        }
    }
}
