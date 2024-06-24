using CryptoTrade.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrade.DbContexts;

public class CryptoTradeContext : DbContext
{
    public CryptoTradeContext(DbContextOptions<CryptoTradeContext> options) : base(options) { }

    public DbSet<Candlestick> Candlesticks { get; set; }

    public DbSet<Interval> Intervals { get; set; }

    public DbSet<Symbol> Symbols { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Interval>()
            .HasData(
            new Interval(1, "1s"),
            new Interval(60, "1m"),
            new Interval(60 * 3, "3m"),
            new Interval(60 * 5, "5m"),
            new Interval(60 * 15, "15m"),
            new Interval(60 * 30, "30m"),
            new Interval(60 * 60, "1h"),
            new Interval(60 * 60 * 2, "2h"),
            new Interval(60 * 60 * 4, "4h"),
            new Interval(60 * 60 * 6, "6h"),
            new Interval(60 * 60 * 8, "8h"),
            new Interval(60 * 60 * 12, "12h"),
            new Interval(60 * 60 * 24, "1d"),
            new Interval(60 * 60 * 24 * 3, "3d"),
            new Interval(60 * 60 * 24 * 7, "1w"),
            new Interval(60 * 60 * 24 * 30, "1M")
            );


        modelBuilder.Entity<Candlestick>()
            .Property(c => c.ClosePrice)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.HighPrice)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.LowPrice)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.OpenPrice)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.QuoteVolume)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.TakerBuyBaseVolume)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.TakerBuyQuoteVolume)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<Candlestick>()
            .Property(c => c.Volume)
            .HasColumnType("decimal(18, 6)");


        modelBuilder.Entity<Candlestick>().HasKey(c => new { c.OpenTime, c.IntervalId, c.SymbolName });


    }
}