using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Steve.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    DiagonalScreen = table.Column<string>(nullable: true),
                    GraphicAdapter = table.Column<string>(nullable: true),
                    HardDiskSpace = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Processor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptops", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laptops");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Emails");
        }
    }
}
