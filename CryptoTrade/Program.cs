using Binance.Net.Clients;
using CryptoTrade.DbContexts;
using CryptoTrade.Helpers;
using CryptoTrade.Repositories;
using CryptoTrade.Service.Binance;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("CryptoTrade.Controllers.CandlestickController", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .CreateLogger();

//builder.Host.UseSerilog();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<BinanceRestClient>();
builder.Services.AddTransient<ICandlestickRepository, CandlestickRepository>();
builder.Services.AddTransient<IIntervalRepository, IntervalRepository>();
builder.Services.AddTransient<ISymbolRepository, SymbolRepository>();
builder.Services.AddTransient<ExchangeData>();



builder.Services.AddDbContext<CryptoTradeContext>(dbContextOptions =>
{
    dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:CryptoTradeConnectionString"]);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();