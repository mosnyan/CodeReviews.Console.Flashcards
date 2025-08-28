namespace Flashcards.mosnyan.Domain.Models;

public class FlashcardStack
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    private List<Flashcard> Cards { get; }

    public FlashcardStack(string subject)
    {
        Id = Guid.NewGuid();
        Subject = subject;
        Cards = [];
    }

    public FlashcardStack(string subject, List<Flashcard> cards)
    {
        Id = Guid.NewGuid();
        Subject = subject;
        Cards = cards;
    }

    public FlashcardStack(Guid id, string subject)
    {
        Id = id;
        Subject = subject;
        Cards = [];
    }

    public FlashcardStack(Guid id, string subject, List<Flashcard> cards)
    {
        Id = id;
        Subject = subject;
        Cards = cards;
    }


    public void AddCard(Flashcard flashcard)
    {
        Cards.Add(flashcard);
    }

    public void AddCards(IEnumerable<Flashcard> cards)
    {
        Cards.AddRange(cards);
    }

    public void RemoveCard(Flashcard flashcard)
    {
        Cards.Remove(flashcard);
    }

    public IReadOnlyList<Flashcard> GetCards()
    {
        return Cards.AsReadOnly();
    }
}