using Scalar.AspNetCore;
using Playtestr.Api.Models;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// GET
app.MapGet("/games", (GameService service) => service.GetAll());
app.MapGet("/games/{id}", (int id, GameService service) =>
{
    var game = service.GetById(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

// POST
app.MapPost("/games", (CreateGameRequest request, GameService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Name is required!");
    if (string.IsNullOrWhiteSpace(request.Description)) return Results.BadRequest("Description is required!");
    
    var game = service.Add(request);
    return Results.Created($"/games/{game.Id}", game);
});

// PUT
app.MapPut("/games/{id}", (int id, CreateGameRequest request, GameService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Name is required!");
    if (string.IsNullOrWhiteSpace(request.Description)) return Results.BadRequest("Description is required!");

    var game = service.Update(id, request);
    if (game is null) return Results.NotFound();

    return Results.Ok(game);
});

// DELETE
app.MapDelete("/games/{id}", (int id, GameService service) =>
{
    var success = service.Delete(id);
    return success ? Results.NoContent() : Results.NotFound();
});

app.Run();