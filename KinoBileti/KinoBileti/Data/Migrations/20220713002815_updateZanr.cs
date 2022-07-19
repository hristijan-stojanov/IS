using Microsoft.EntityFrameworkCore.Migrations;

namespace KinoBileti.Data.Migrations
{
    public partial class updateZanr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "zanr",
                table: "Bilets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zanr",
                table: "Bilets");
        }
    }
}
