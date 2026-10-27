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
    private const int MissingId = 999;

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
    public async Task GetAllAsync_ReturnsAllGames()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        context.Games.AddRange(
            new Game { Name = "First game", Description = "lol", ReleaseYear = 2024, OwnerId = owner.Id },
            new Game { Name = "Second game", Description = "lol", ReleaseYear = 2025, OwnerId = owner.Id });
        await context.SaveChangesAsync();

        var service = new GameService(context);

        // Act
        var games = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, games.Count);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameExists_ReturnsGame()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, OwnerId = owner.Id };
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var service = new GameService(context);

        // Act
        var result = await service.GetByIdAsync(game.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
        Assert.Equal("Test game", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGameDoesNotExist_ReturnsNull()
    {
        // Arrange
        var context = CreateContext();
        var service = new GameService(context);

        // Act
        var result = await service.GetByIdAsync(MissingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_SavesGameWithCallerAsOwner()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var service = new GameService(context);
        var request = new CreateGameRequest("New game", "Brand new", 2026);

        // Act
        var result = await service.AddAsync(request, owner.Id);

        // Assert
        Assert.Equal("New game", result.Name);

        context.ChangeTracker.Clear();
        var saved = await context.Games.FindAsync(result.Id);
        Assert.NotNull(saved);
        Assert.Equal(owner.Id, saved.OwnerId);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsOwner_UpdatesGame()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, OwnerId = owner.Id };
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var service = new GameService(context);
        var request = new CreateGameRequest("Renamed", "New description", 2026);

        // Act
        var result = await service.UpdateAsync(game.Id, owner.Id, request);

        // Assert
        Assert.Equal(OperationStatus.Success, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal("Renamed", result.Value.Name);

        context.ChangeTracker.Clear();
        var saved = await context.Games.FindAsync(game.Id);
        Assert.Equal("Renamed", saved!.Name);
        Assert.Equal("New description", saved.Description);
        Assert.Equal(2026, saved.ReleaseYear);
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
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsNotOwner_DoesNotChangeGame()
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
        var request = new CreateGameRequest("Hacked", "Hacked", 2000);

        // Act
        await service.UpdateAsync(game.Id, stranger.Id, request);

        // Assert
        context.ChangeTracker.Clear();
        var saved = await context.Games.FindAsync(game.Id);
        Assert.Equal("Test game", saved!.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenGameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var service = new GameService(context);
        var request = new CreateGameRequest("Test", "Test", 2024);

        // Act
        var result = await service.UpdateAsync(MissingId, owner.Id, request);

        // Assert
        Assert.Equal(OperationStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserIsOwner_DeletesGame()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, OwnerId = owner.Id };
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var service = new GameService(context);

        // Act
        var status = await service.DeleteAsync(game.Id, owner.Id);

        // Assert
        Assert.Equal(OperationStatus.Success, status);
        Assert.False(await context.Games.AnyAsync(g => g.Id == game.Id));
    }

    [Fact]
    public async Task DeleteAsync_WhenUserIsNotOwner_ReturnsForbidden()
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

        // Act
        var status = await service.DeleteAsync(game.Id, stranger.Id);

        // Assert
        Assert.Equal(OperationStatus.Forbidden, status);
        Assert.True(await context.Games.AnyAsync(g => g.Id == game.Id));
    }

    [Fact]
    public async Task DeleteAsync_WhenGameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        var service = new GameService(context);

        // Act
        var status = await service.DeleteAsync(MissingId, owner.Id);

        // Assert
        Assert.Equal(OperationStatus.NotFound, status);
    }
}
