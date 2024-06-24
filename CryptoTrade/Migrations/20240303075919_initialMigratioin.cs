using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrade.Migrations
{
    /// <inheritdoc />
    public partial class initialMigratioin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGenerated",
                table: "Candlesticks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGenerated",
                table: "Candlesticks");
        }
    }
}
