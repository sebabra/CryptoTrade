using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrade.Migrations
{
    /// <inheritdoc />
    public partial class updateCandlestickSymbolName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SymbolId",
                table: "Candlesticks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SymbolId",
                table: "Candlesticks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
