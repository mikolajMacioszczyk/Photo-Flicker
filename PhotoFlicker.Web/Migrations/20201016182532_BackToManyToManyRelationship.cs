using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoFlicker.Web.Migrations
{
    public partial class BackToManyToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagItems_PhotoItems_PhotoId",
                table: "TagItems");

            migrationBuilder.DropIndex(
                name: "IX_TagItems_PhotoId",
                table: "TagItems");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "TagItems");

            migrationBuilder.CreateTable(
                name: "PhotoTag",
                columns: table => new
                {
                    PhotosId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTag", x => new { x.PhotosId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PhotoTag_PhotoItems_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "PhotoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoTag_TagItems_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTag_TagsId",
                table: "PhotoTag",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoTag");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "TagItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagItems_PhotoId",
                table: "TagItems",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagItems_PhotoItems_PhotoId",
                table: "TagItems",
                column: "PhotoId",
                principalTable: "PhotoItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
