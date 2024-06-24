using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CryptoTrade.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Intervals",
                columns: table => new
                {
                    IntervalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.IntervalId);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseAsset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuoteAsset = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Candlesticks",
                columns: table => new
                {
                    OpenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    HighPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LowPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CloseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuoteVolume = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TradeCount = table.Column<int>(type: "int", nullable: false),
                    TakerBuyBaseVolume = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TakerBuyQuoteVolume = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IntervalId = table.Column<int>(type: "int", nullable: false),
                    SymbolName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SymbolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candlesticks", x => x.OpenTime);
                    table.ForeignKey(
                        name: "FK_Candlesticks_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "IntervalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candlesticks_Symbols_SymbolName",
                        column: x => x.SymbolName,
                        principalTable: "Symbols",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Intervals",
                columns: new[] { "IntervalId", "Name" },
                values: new object[,]
                {
                    { 1, "1s" },
                    { 60, "1m" },
                    { 180, "3m" },
                    { 300, "5m" },
                    { 900, "15m" },
                    { 1800, "30m" },
                    { 3600, "1h" },
                    { 7200, "2h" },
                    { 14400, "4h" },
                    { 21600, "6h" },
                    { 28800, "8h" },
                    { 43200, "12h" },
                    { 86400, "1d" },
                    { 259200, "3d" },
                    { 604800, "1w" },
                    { 2592000, "1M" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candlesticks_IntervalId",
                table: "Candlesticks",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Candlesticks_SymbolName",
                table: "Candlesticks",
                column: "SymbolName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candlesticks");

            migrationBuilder.DropTable(
                name: "Intervals");

            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
