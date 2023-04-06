using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADO.net.Migrations
{
    /// <inheritdoc />
    public partial class AddedManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Secname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdMainDep = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdSecDep = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdChief = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FiredDt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduct = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdManager = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cnt = table.Column<int>(type: "int", nullable: false),
                    SaleDt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteDt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
