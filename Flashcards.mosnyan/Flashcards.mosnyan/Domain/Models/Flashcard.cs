using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.mosnyan.Domain.Models;

public class Flashcard
{
    [Column("cardId")]
    public Guid Id { get; set; }
    public string Prompt { get; set; }
    public string Answer { get; set; }
    public Guid StackId { get; set; }
    

    public Flashcard(string prompt, string answer, Guid stackId)
    {
        Id = Guid.NewGuid();
        Prompt = prompt;
        Answer = answer;
        StackId = stackId;
    }

    public Flashcard(Guid cardId, string prompt, string answer, Guid stackId)
    {
        Id = cardId;
        Prompt = prompt;
        Answer = answer;
        StackId = stackId;
    }
}