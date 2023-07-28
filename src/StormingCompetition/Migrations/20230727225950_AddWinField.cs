using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StormingCompetition.Migrations
{
    /// <inheritdoc />
    public partial class AddWinField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Win",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Win",
                table: "Users");
        }
    }
}
