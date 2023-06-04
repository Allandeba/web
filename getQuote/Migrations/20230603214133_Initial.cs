using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Person",
                    columns: table =>
                        new
                        {
                            PersonId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            FirstName = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            LastName = table
                                .Column<string>(
                                    type: "varchar(100)",
                                    maxLength: 100,
                                    nullable: false
                                )
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            CreationDate = table.Column<DateTime>(
                                type: "timestamp",
                                nullable: false,
                                defaultValueSql: "CURRENT_TIMESTAMP"
                            )
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Person", x => x.PersonId);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Contact",
                    columns: table =>
                        new
                        {
                            ContactId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            Email = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Phone = table
                                .Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            PersonId = table.Column<int>(type: "int", nullable: true)
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Contact", x => x.ContactId);
                        table.ForeignKey(
                            name: "FK_Contact_Person_PersonId",
                            column: x => x.PersonId,
                            principalTable: "Person",
                            principalColumn: "PersonId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Document",
                    columns: table =>
                        new
                        {
                            DocumentId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            DocumentType = table.Column<int>(type: "int", nullable: false),
                            Document = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            PersonId = table.Column<int>(type: "int", nullable: true)
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Document", x => x.DocumentId);
                        table.ForeignKey(
                            name: "FK_Document_Person_PersonId",
                            column: x => x.PersonId,
                            principalTable: "Person",
                            principalColumn: "PersonId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PersonId",
                table: "Contact",
                column: "PersonId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Document_PersonId",
                table: "Document",
                column: "PersonId",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Contact");

            migrationBuilder.DropTable(name: "Document");

            migrationBuilder.DropTable(name: "Person");
        }
    }
}
