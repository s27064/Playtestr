using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Playtests;

public class PlaytestService(AppDbContext context) : IPlaytestService
{
    public async Task<PlaytestResponse?> AddAsync(int gameId, CreatePlaytestRequest request)
    {
        if (!await context.Games.AnyAsync(g => g.Id == gameId)) return null;

        var playtest = new Playtest
        {
            Title = request.Title,
            Description = request.Description,
            GameId = gameId
        };

        context.Playtests.Add(playtest);
        await context.SaveChangesAsync();
        return playtest.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var playtest = await context.Playtests.FindAsync(id);
        if (playtest is null) return false;

        context.Playtests.Remove(playtest);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IReadOnlyList<PlaytestResponse>> GetAllAsync() =>
        await context.Playtests
            .Select(p => p.ToResponse())
            .ToListAsync();

    public async Task<IReadOnlyList<PlaytestResponse>?> GetByGameIdAsync(int gameId)
    {
        if (!await context.Games.AnyAsync(g => g.Id == gameId)) return null;

        return await context.Playtests
            .Where(p => p.GameId == gameId)
            .Select(p => p.ToResponse())
            .ToListAsync();
    }

    public async Task<PlaytestResponse?> GetByIdAsync(int id)
    {
        var playtest = await context.Playtests.FindAsync(id);
        return playtest?.ToResponse();
    }

    public async Task<PlaytestResponse?> UpdateAsync(int id, CreatePlaytestRequest request)
    {
        var playtest = await context.Playtests.FindAsync(id);
        if (playtest is null) return null;

        playtest.Title = request.Title;
        playtest.Description = request.Description;

        await context.SaveChangesAsync();
        return playtest.ToResponse();
    }
}