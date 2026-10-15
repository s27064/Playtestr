using Scalar.AspNetCore;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services;
using Playtestr.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGameService, GameService>();
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

// GET
app.MapGet("/games", async (IGameService service) => await service.GetAllAsync());
app.MapGet("/games/{id}", async (int id, IGameService service) =>
{
    var game = await service.GetByIdAsync(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

// POST
app.MapPost("/games", async (CreateGameRequest request, IGameService service) =>
{
    var game = await service.AddAsync(request);
    return Results.Created($"/games/{game.Id}", game);
});

// PUT
app.MapPut("/games/{id}", async (int id, CreateGameRequest request, IGameService service) =>
{
    var game = await service.UpdateAsync(id, request);
    if (game is null) return Results.NotFound();

    return Results.Ok(game);
});

// DELETE
app.MapDelete("/games/{id}", async (int id, IGameService service) =>
{
    var success = await service.DeleteAsync(id);
    return success ? Results.NoContent() : Results.NotFound();
});

app.Run();