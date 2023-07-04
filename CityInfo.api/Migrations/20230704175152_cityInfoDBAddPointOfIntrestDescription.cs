using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.api.Migrations
{
    /// <inheritdoc />
    public partial class cityInfoDBAddPointOfIntrestDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "pointOfIntrest",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "pointOfIntrest");
        }
    }
}
