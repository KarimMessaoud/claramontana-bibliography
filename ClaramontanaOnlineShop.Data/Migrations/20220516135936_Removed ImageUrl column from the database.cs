using Microsoft.EntityFrameworkCore.Migrations;

namespace ClaramontanaOnlineShop.Data.Migrations
{
    public partial class RemovedImageUrlcolumnfromthedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPages",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "NumberOfPages",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
