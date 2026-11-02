using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Models;

namespace Playtestr.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Playtest> Playtests => Set<Playtest>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.Entity<Game>().ToTable(t =>
            t.HasCheckConstraint("CK_Games_ReleaseYear", "\"ReleaseYear\" BETWEEN 1950 AND 2100"));

        modelBuilder.Entity<Question>().ToTable(t =>
            t.HasCheckConstraint("CK_Questions_Type", "\"Type\" IN (0, 1)"));

        modelBuilder.Entity<Answer>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Answers_Rating", "\"Rating\" BETWEEN 1 AND 5");
            // an answer is either a rating or a text, never both or neither
            t.HasCheckConstraint("CK_Answers_RatingOrText", "(\"Rating\" IS NULL) <> (\"Text\" IS NULL)");
        });
    }
}