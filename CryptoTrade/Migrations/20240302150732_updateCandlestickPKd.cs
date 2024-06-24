using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrade.Migrations
{
    /// <inheritdoc />
    public partial class updateCandlestickPKd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Candlesticks",
                table: "Candlesticks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candlesticks",
                table: "Candlesticks",
                columns: new[] { "OpenTime", "IntervalId", "SymbolName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Candlesticks",
                table: "Candlesticks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candlesticks",
                table: "Candlesticks",
                column: "OpenTime");
        }
    }
}
