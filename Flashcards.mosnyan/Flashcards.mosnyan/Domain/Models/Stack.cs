namespace Flashcards.mosnyan.Domain.Models;

public class Stack
{
    public Guid Id { get; }
    public string Subject { get; }
    private List<Card> Cards { get; }

    public Stack(string subject)
    {
        Id = Guid.NewGuid();
        Subject = subject;
        Cards = [];
    }

    public Stack(string subject, List<Card> cards)
    {
        Id = Guid.NewGuid();
        Subject = subject;
        Cards = cards;
    }

    public Stack(Guid id, string subject, List<Card> cards)
    {
        Id = id;
        Subject = subject;
        Cards = cards;
    }


    public Stack AddCard(Card card)
    {
        var newCards = new List<Card>(Cards) { card };
        return new Stack(Id, Subject, newCards);
    }

    public Stack AddCards(IEnumerable<Card> cards)
    {
        var newCards = new List<Card>(Cards);
        newCards.AddRange(cards);
        return new Stack(Id, Subject, newCards);
    }

    public Stack RemoveCard(Card card)
    {
        var newCards = new List<Card>(Cards);
        newCards.Remove(card);
        return new Stack(Id, Subject, newCards);
    }

    public IReadOnlyList<Card> GetCards()
    {
        return Cards.AsReadOnly();
    }
}