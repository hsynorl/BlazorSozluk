using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorSozluk.Infrastructure.Persistence.Migrations
{
    public partial class initMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bisey",
                schema: "dbo",
                table: "entry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bisey",
                schema: "dbo",
                table: "entry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
