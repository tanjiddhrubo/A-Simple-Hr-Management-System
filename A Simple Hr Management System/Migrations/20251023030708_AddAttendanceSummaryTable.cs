using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Simple_Hr_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceSummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dtYear = table.Column<int>(type: "integer", nullable: false),
                    dtMonth = table.Column<int>(type: "integer", nullable: false),
                    Present = table.Column<int>(type: "integer", nullable: false),
                    Late = table.Column<int>(type: "integer", nullable: false),
                    Absent = table.Column<int>(type: "integer", nullable: false),
                    ComId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceSummaries_Companies_ComId",
                        column: x => x.ComId,
                        principalTable: "Companies",
                        principalColumn: "ComId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceSummaries_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSummaries_ComId",
                table: "AttendanceSummaries",
                column: "ComId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSummaries_EmpId",
                table: "AttendanceSummaries",
                column: "EmpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceSummaries");
        }
    }
}
