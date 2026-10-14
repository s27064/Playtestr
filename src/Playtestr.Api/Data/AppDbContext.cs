using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Models;

namespace Playtestr.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
}