using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbracePets.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "TB_Usuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "TB_Usuario");
        }
    }
}
