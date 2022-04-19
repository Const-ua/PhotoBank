using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoBank.Migrations
{
    public partial class AddKeyToPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuthor",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuthor",
                table: "AspNetUsers");
        }
    }
}
