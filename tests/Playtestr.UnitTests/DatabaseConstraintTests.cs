using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Models;

namespace Playtestr.UnitTests;

public class DatabaseConstraintTests
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
    public async Task Submission_SameUserTwiceForOnePlaytest_IsRejected()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, Owner = user };
        var playtest = new Playtest { Title = "Test playtest", Description = "lol", Game = game };
        context.Playtests.Add(playtest);
        await context.SaveChangesAsync();

        context.Submissions.Add(new Submission { UserId = user.Id, PlaytestId = playtest.Id });
        await context.SaveChangesAsync();

        // Act
        context.Submissions.Add(new Submission { UserId = user.Id, PlaytestId = playtest.Id });

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Fact]
    public async Task Answer_SameQuestionTwiceInOneSubmission_IsRejected()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, Owner = user };
        var playtest = new Playtest { Title = "Test playtest", Description = "lol", Game = game };
        var question = new Question { Text = "How fun was it?", Type = QuestionType.Rating, Playtest = playtest };
        context.Questions.Add(question);
        await context.SaveChangesAsync();

        // Act
        context.Submissions.Add(new Submission
        {
            UserId = user.Id,
            PlaytestId = playtest.Id,
            Answers =
            [
                new Answer { QuestionId = question.Id, Rating = 3 },
                new Answer { QuestionId = question.Id, Rating = 4 }
            ]
        });

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task Answer_RatingOutOfRange_IsRejected(int rating)
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, Owner = user };
        var playtest = new Playtest { Title = "Test playtest", Description = "lol", Game = game };
        var question = new Question { Text = "How fun was it?", Type = QuestionType.Rating, Playtest = playtest };
        context.Questions.Add(question);
        await context.SaveChangesAsync();

        // Act
        context.Submissions.Add(new Submission
        {
            UserId = user.Id,
            PlaytestId = playtest.Id,
            Answers = [new Answer { QuestionId = question.Id, Rating = rating }]
        });

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Theory]
    [InlineData(3, "Also some text")]
    [InlineData(null, null)]
    public async Task Answer_WithBothOrNeitherRatingAndText_IsRejected(int? rating, string? text)
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        var game = new Game { Name = "Test game", Description = "lol", ReleaseYear = 2024, Owner = user };
        var playtest = new Playtest { Title = "Test playtest", Description = "lol", Game = game };
        var question = new Question { Text = "How fun was it?", Type = QuestionType.Rating, Playtest = playtest };
        context.Questions.Add(question);
        await context.SaveChangesAsync();

        // Act
        context.Submissions.Add(new Submission
        {
            UserId = user.Id,
            PlaytestId = playtest.Id,
            Answers = [new Answer { QuestionId = question.Id, Rating = rating, Text = text }]
        });

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Fact]
    public async Task Game_ReleaseYearOutOfRange_IsRejected()
    {
        // Arrange
        var context = CreateContext();
        var owner = new User { Email = "owner@test.com", Username = "owner", PasswordHash = "x" };
        context.Users.Add(owner);
        await context.SaveChangesAsync();

        // Act
        context.Games.Add(new Game { Name = "Old game", Description = "lol", ReleaseYear = 1800, OwnerId = owner.Id });

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }
}
