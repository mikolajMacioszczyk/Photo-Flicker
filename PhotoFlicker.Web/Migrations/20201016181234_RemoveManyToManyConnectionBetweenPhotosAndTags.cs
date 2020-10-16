using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoFlicker.Web.Migrations
{
    public partial class RemoveManyToManyConnectionBetweenPhotosAndTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoTag");

            migrationBuilder.DeleteData(
                table: "PhotoItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PhotoItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PhotoItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PhotoItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TagItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TagItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TagItems",
                keyColumn: "Id",
                keyValue: 3);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                    MarkedPhotosId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTag", x => new { x.MarkedPhotosId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PhotoTag_PhotoItems_MarkedPhotosId",
                        column: x => x.MarkedPhotosId,
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

            migrationBuilder.InsertData(
                table: "PhotoItems",
                columns: new[] { "Id", "Path" },
                values: new object[,]
                {
                    { 1, "https://planetescape.pl//app/uploads/2018/10/Wielki-Kanion-3.jpg" },
                    { 2, "https://www.ef.pl/sitecore/__/~/media/universal/pg/8x5/destination/US_US-CA_LAX_1.jpg" },
                    { 3, "https://i.wpimg.pl/1500x0/d.wpimg.pl/1036153311-705207955/zorza-polarna.jpg" },
                    { 4, "https://www.banita.travel.pl/wp-content/uploads/2019/10/zorza-polarna-norwegia1-1920x1282.jpg" }
                });

            migrationBuilder.InsertData(
                table: "TagItems",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Kanion" },
                    { 2, "USA" },
                    { 3, "Zorza" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTag_TagsId",
                table: "PhotoTag",
                column: "TagsId");
        }
    }
}
