using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class KinderGartenInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
<<<<<<<< HEAD:TARge25Shop/TARge25Shop.Data/Migrations/20260922154855_Init.cs
                name: "FileToApis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExistingFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpaceshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileToApis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spaceships",
========
                name: "Kindergartens",
>>>>>>>> origin/Kindergarden:TARge25Shop/TARge25Shop.Data/Migrations/20260923150142_KinderGartenInit.cs
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    KindergartenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kindergartens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
<<<<<<<< HEAD:TARge25Shop/TARge25Shop.Data/Migrations/20260922154855_Init.cs
                name: "FileToApis");

            migrationBuilder.DropTable(
                name: "Spaceships");
========
                name: "Kindergartens");
>>>>>>>> origin/Kindergarden:TARge25Shop/TARge25Shop.Data/Migrations/20260923150142_KinderGartenInit.cs
        }
    }
}
