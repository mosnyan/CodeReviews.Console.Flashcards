namespace Flashcards.mosnyan.Domain.Models;

public class Session(Guid id, DateTime timeStamp, double score, Guid stackId)
{
    public Guid Id { get; set; } = id;
    public DateTime TimeStamp { get; set; } = timeStamp;
    public double Score { get; set; } = score;
    public Guid StackId { get; set; } = stackId;
}