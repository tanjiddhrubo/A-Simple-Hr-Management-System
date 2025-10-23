using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Simple_Hr_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dtYear = table.Column<int>(type: "integer", nullable: false),
                    dtMonth = table.Column<int>(type: "integer", nullable: false),
                    Gross = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Basic = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    HRent = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Medical = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AbsentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ComId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salaries_Companies_ComId",
                        column: x => x.ComId,
                        principalTable: "Companies",
                        principalColumn: "ComId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salaries_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_ComId",
                table: "Salaries",
                column: "ComId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmpId",
                table: "Salaries",
                column: "EmpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salaries");
        }
    }
}
