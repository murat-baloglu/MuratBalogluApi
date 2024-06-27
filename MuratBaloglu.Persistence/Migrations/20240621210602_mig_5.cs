using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuratBaloglu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Specialties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecialityCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialityCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_CategoryId",
                table: "Specialties",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_SpecialityCategories_CategoryId",
                table: "Specialties",
                column: "CategoryId",
                principalTable: "SpecialityCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_SpecialityCategories_CategoryId",
                table: "Specialties");

            migrationBuilder.DropTable(
                name: "SpecialityCategories");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_CategoryId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Specialties");
        }
    }
}
