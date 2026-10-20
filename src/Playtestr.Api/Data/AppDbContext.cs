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
}