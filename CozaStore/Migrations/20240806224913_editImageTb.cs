using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozaStore.Migrations
{
    /// <inheritdoc />
    public partial class editImageTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Images");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
