using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SW.I18nServices.Web.Migrations
{
    public partial class Place : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<string>(maxLength: 2, nullable: true),
                    Region1 = table.Column<string>(maxLength: 80, nullable: true),
                    Region2 = table.Column<string>(maxLength: 80, nullable: true),
                    Region3 = table.Column<string>(maxLength: 80, nullable: true),
                    Region4 = table.Column<string>(maxLength: 80, nullable: true),
                    Locality = table.Column<string>(maxLength: 80, nullable: true),
                    Postcode = table.Column<string>(maxLength: 15, nullable: true),
                    Suburb = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<decimal>(nullable: false),
                    Latitude = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Place");
        }
    }
}
