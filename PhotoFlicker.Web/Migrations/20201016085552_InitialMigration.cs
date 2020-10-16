using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PhotoFlicker.Web.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhotoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagItems", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoTag");

            migrationBuilder.DropTable(
                name: "PhotoItems");

            migrationBuilder.DropTable(
                name: "TagItems");
        }
    }
}
