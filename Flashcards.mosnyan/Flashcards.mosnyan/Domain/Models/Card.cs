namespace Flashcards.mosnyan.Domain.Models;

public class Card
{
    public Guid Id { get; }
    public string Prompt { get; }
    public string Answer { get; }
    public Guid StackId { get; }

    public Card(string prompt, string answer, Guid stackId)
    {
        Id = Guid.NewGuid();
        Prompt = prompt;
        Answer = answer;
        StackId = stackId;
    }

    public Card(Guid id, string prompt, string answer, Guid stackId)
    {
        Id = id;
        Prompt = prompt;
        Answer = answer;
        StackId = stackId;
    }
}