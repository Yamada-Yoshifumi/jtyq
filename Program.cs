using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Microsoft.Extensions;
using jtyq.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(ConfigurationExtensions.GetConnectionString(builder.Configuration, "Default")));
builder.Services.AddTransient<jtyqContext>(_ => new jtyqContext(ConfigurationExtensions.GetConnectionString(builder.Configuration, "Default")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
