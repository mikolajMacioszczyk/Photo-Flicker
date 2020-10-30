using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoFlicker.Web.Migrations
{
    public partial class AddDescriptionColumnToPhotoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PhotoItems",
                type: "character varying(50000)",
                maxLength: 50000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PhotoItems");
        }
    }
}
