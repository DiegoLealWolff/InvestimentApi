using InvestimentApi.Application.Services;
using InvestimentApi.Application.Services.Interfaces;
using InvestimentApi.Domain.Factories;
using InvestimentApi.Domain.Interfaces;
using InvestimentApi.Domain.Interfaces.Strategies;
using InvestimentApi.Domain.Strategies;
using InvestimentApi.Infrastructure.Data;
using InvestimentApi.Infrastructure.Repositories;
using InvestimentApi.Web.HostedServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderBookService, OrderBookService>();
builder.Services.AddScoped<IOrderStrategyFactory, OrderStrategyFactory>();
builder.Services.AddScoped<OrderFactory>();
builder.Services.AddHostedService<OrderBookBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();