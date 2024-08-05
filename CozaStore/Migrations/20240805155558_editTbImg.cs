using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozaStore.Migrations
{
    /// <inheritdoc />
    public partial class editTbImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Images");
        }
    }
}
