using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;
using Playtestr.Api.Services;
using Playtestr.Api.Services.Games;

namespace Playtestr.UnitTests;

public class GameServiceTests
{
    private static AppDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsNotOwner_ReturnsForbidden()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        var stranger = new User { Email = "stranger@test.com", Username = "stranger", PasswordHash = "x" };
        context.Users.AddRange(owner, stranger);
        await context.SaveChangesAsync();

        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, OwnerId = owner.Id };
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var service = new GameService(context);
        var request = new CreateGameRequest("Test", "Test", 2024);

        // Act
        var result = await service.UpdateAsync(game.Id, stranger.Id, request);

        // Assert
        Assert.Equal(OperationStatus.Forbidden, result.Status);
    }
}