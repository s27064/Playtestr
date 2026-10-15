namespace Playtestr.Api.Models;

public class Playtest
{
    public int Id {get; set;}
    public string Title {get; set;} = "";
    public string Description {get; set;} = "";
    public DateTime CreatedAt {get; set;}

    //Game
    public int GameId {get; set;}
    public Game Game {get; set;} = null!;
}
