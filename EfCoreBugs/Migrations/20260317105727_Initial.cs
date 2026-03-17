using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCoreBugs.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                }
            );

            migrationBuilder.AddForeignKey(
                "FK_Item_ParentId",
                "Items",
                "ParentId",
                "Items",
                null,
                null,
                "Id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Item_ParentId", "Items");
            migrationBuilder.DropTable(name: "Items");
        }
    }
}
