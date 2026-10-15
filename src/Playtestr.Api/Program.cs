using Scalar.AspNetCore;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services;
using Playtestr.Api.Data;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Endpoints;
using Playtestr.Api.Services.Playtests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPlaytestService, PlaytestService>();
builder.Services.AddValidation();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//Endpoints
app.MapGameEndpoints();
app.MapPlaytestEndpoints();

app.Run();