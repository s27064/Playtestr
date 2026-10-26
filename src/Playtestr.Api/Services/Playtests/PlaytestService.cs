using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Playtests;

public class PlaytestService(AppDbContext context) : IPlaytestService
{
    public async Task<OperationResult<PlaytestResponse>> AddAsync(int gameId, int userId, CreatePlaytestRequest request)
    {
        var game = await context.Games.FindAsync(gameId);

        if (game is null) return new(OperationStatus.NotFound);
        if (game.OwnerId != userId) return new(OperationStatus.Forbidden);

        var playtest = new Playtest
        {
            Title = request.Title,
            Description = request.Description,
            GameId = gameId
        };

        context.Playtests.Add(playtest);
        await context.SaveChangesAsync();
        return new OperationResult<PlaytestResponse>(OperationStatus.Success, playtest.ToResponse());
    }

    public async Task<OperationStatus> DeleteAsync(int id, int userId)
    {
        var playtest = await context.Playtests
        .Include(p => p.Game)
        .FirstOrDefaultAsync(p => p.Id == id);
        
        if (playtest is null) return OperationStatus.NotFound;
        if (playtest.Game.OwnerId != userId) return OperationStatus.Forbidden;

        context.Playtests.Remove(playtest);
        await context.SaveChangesAsync();
        return OperationStatus.Success;
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

    public async Task<OperationResult<PlaytestResponse>> UpdateAsync(int id, int userId, CreatePlaytestRequest request)
    {
        var playtest = await context.Playtests
        .Include(p => p.Game)
        .FirstOrDefaultAsync(p => p.Id == id);
        
        if (playtest is null) return new(OperationStatus.NotFound);
        if (playtest.Game.OwnerId != userId) return new(OperationStatus.Forbidden);

        playtest.Title = request.Title;
        playtest.Description = request.Description;

        await context.SaveChangesAsync();
        return new OperationResult<PlaytestResponse>(OperationStatus.Success, playtest.ToResponse());
    }
}