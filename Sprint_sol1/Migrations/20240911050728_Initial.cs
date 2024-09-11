using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint_sol1.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Dept_ID = table.Column<int>(type: "int", nullable: false)
                       ,
                    Dept_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Dept_ID);
                });

            migrationBuilder.CreateTable(
                name: "GradeMasters",
                columns: table => new
                {
                    Grade_Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Min_Salary = table.Column<int>(type: "int", nullable: false),
                    Max_Salary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeMasters", x => x.Grade_Code);
                });

            migrationBuilder.CreateTable(
                name: "UserMasters",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMasters", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Emp_ID = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    Emp_First_Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Emp_Last_Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Emp_Date_of_Birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Emp_Date_of_Joining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Emp_Dept_ID = table.Column<int>(type: "int", nullable: false),
                    Emp_Grade = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Emp_Designation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Emp_Basic = table.Column<int>(type: "int", nullable: false),
                    Emp_Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Marital_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emp_Home_Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Emp_Contact_Num = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Emp_ID);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_Emp_Dept_ID",
                        column: x => x.Emp_Dept_ID,
                        principalTable: "Departments",
                        principalColumn: "Dept_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_GradeMasters_Emp_Grade",
                        column: x => x.Emp_Grade,
                        principalTable: "GradeMasters",
                        principalColumn: "Grade_Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_UserMasters_Emp_ID",
                        column: x => x.Emp_ID,
                        principalTable: "UserMasters",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Emp_Dept_ID",
                table: "Employees",
                column: "Emp_Dept_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Emp_Grade",
                table: "Employees",
                column: "Emp_Grade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "GradeMasters");

            migrationBuilder.DropTable(
                name: "UserMasters");
        }
    }
}
