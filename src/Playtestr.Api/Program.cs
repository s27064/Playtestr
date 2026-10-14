using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var gameList = new List<Game>
{
    new Game(0, "Cyberpunk", "COOL SHOOTA", 2019),
    new Game(1, "Witcher 3", "Old guy with sword", 2015),
    new Game(2, "Counter Strike", "Best shoota", 2010)
};

app.MapGet("/games", () =>
{
    return gameList;
})
.WithName("GetGames");

app.MapGet("/games/{id}", (int id) =>
{
    var game = gameList.FirstOrDefault(g=>g.id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName("GetGame");

app.MapPost("/games", (CreateGameRequest request) =>
{
    if (string.IsNullOrEmpty(request.Name)) return Results.BadRequest("Name is required");
    if (string.IsNullOrEmpty(request.Description)) return Results.BadRequest("Description is required");

    var game = new Game(gameList.Count, request.Name, request.Description, request.releaseYear);
    gameList.Add(game);

    return Results.Ok(game);
})
.WithName("PostGame");

app.MapPut("/games/{id}", (int id, CreateGameRequest request) =>
{
    var index = gameList.FindIndex(g => g.id == id);
    if (index == -1) return Results.NotFound("Game with this id has not been found!");

    var newGame = new Game(id, request.Name, request.Description, request.releaseYear);
    gameList[index] = newGame;

    return Results.Ok(newGame);
})
.WithName("PutGame");

app.MapDelete("/games/{id}", (int id) =>
{
    var index = gameList.FindIndex(g => g.id == id);
    if (index == -1) return Results.NotFound("Game with this id has not been found!");

    gameList.RemoveAt(index);
    return Results.NoContent();
})
.WithName("DeleteGame");

app.Run();

record Game(int id, string Name, string Description, int releaseYear);
record CreateGameRequest(string Name, string Description, int releaseYear);
