using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Simple_Hr_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ComId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComName = table.Column<string>(type: "text", nullable: false),
                    Basic = table.Column<decimal>(type: "numeric", nullable: false),
                    Hrent = table.Column<decimal>(type: "numeric", nullable: false),
                    Medical = table.Column<decimal>(type: "numeric", nullable: false),
                    IsInactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ComId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
