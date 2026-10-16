namespace Playtestr.Api.Models;

public class Answer
{
    public int Id {get; set;}
    public int? Rating { get; set; }
    public string? Text { get; set; }
    
    public int ResponseId {get; set;}
    public Response Response {get; set;} = null!;

    public int QuestionId {get; set;}
    public Question Question {get; set;} = null!;
}
