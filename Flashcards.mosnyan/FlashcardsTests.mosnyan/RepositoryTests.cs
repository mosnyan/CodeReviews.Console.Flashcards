using MySqlConnector;
using Dapper;
using Flashcards.mosnyan.Domain.Models;
using Flashcards.mosnyan.Infrastructure.Repository;

namespace FlashcardsTests.mosnyan;

public class RepositoryTests
{
    private const String ConnectionString = "Server=localhost;" +
                                            "Port=3306;" +
                                            "Database=flashcards_tests;" +
                                            "Uid=user;" +
                                            "Pwd=password;";

    private StackRepository repo = new(ConnectionString);
    
    [SetUp]
    public void SetUp()
    {
        using var connection = new MySqlConnection(ConnectionString);
        var query = "DELETE FROM stacks;";
        connection.Execute(query);
    }

    [Test]
    public void CreateStack()
    {
        FlashcardStack flashcardStack = new("French", []);
        Assert.That(repo.CreateNewStack(flashcardStack), Is.True);
    }

    [Test]
    public void CreateStackWithCards()
    {
        FlashcardStack flashcardStack = new("French");
        
        List <Flashcard> cards =
        [
            new Flashcard("Dog", "Chien", flashcardStack.Id),
            new Flashcard("Cat", "Chat", flashcardStack.Id)
        ];

        flashcardStack.AddCards(cards);
        
        Assert.That(repo.CreateNewStack(flashcardStack), Is.True);
    }

    [Test]
    public void GetAllStacks()
    {
        FlashcardStack flashcardStackToCreate = new("French", []);
        repo.CreateNewStack(flashcardStackToCreate);

        var stacks = repo.ReadAllStacks().ToList();
        
        Assert.That(stacks, Has.Count.EqualTo(1));

        var resultStack = stacks.ElementAt(0);
        
        Assert.Multiple(() =>
        {
            Assert.That(resultStack.Id, Is.EqualTo(flashcardStackToCreate.Id));
            Assert.That(resultStack.Subject, Is.EqualTo(flashcardStackToCreate.Subject));
            Assert.That(resultStack.GetCards().ToList(), Has.Count.EqualTo(0));
        });
    }
    
    [Test]
    public void GetAllStacksWithCards()
    {
        FlashcardStack flashcardStackToCreate = new("French");
        
        List <Flashcard> cards =
        [
            new Flashcard("Dog", "Chien", flashcardStackToCreate.Id),
            new Flashcard("Cat", "Chat", flashcardStackToCreate.Id)
        ];

        flashcardStackToCreate.AddCards(cards);
        repo.CreateNewStack(flashcardStackToCreate);

        var stacks = repo.ReadAllStacks().ToList();
        
        Assert.That(stacks, Has.Count.EqualTo(1));

        var resultStack = stacks.ElementAt(0);
        
        Assert.Multiple(() =>
        {
            Assert.That(resultStack.Id, Is.EqualTo(flashcardStackToCreate.Id));
            Assert.That(resultStack.Subject, Is.EqualTo(flashcardStackToCreate.Subject));
            Assert.That(resultStack.GetCards().ToList(), Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void UpdateStackWithNewSubject()
    {
        FlashcardStack flashcardStackToCreate = new("French");
        
        List <Flashcard> cards =
        [
            new Flashcard("Dog", "Chien", flashcardStackToCreate.Id),
            new Flashcard("Cat", "Chat", flashcardStackToCreate.Id)
        ];

        flashcardStackToCreate.AddCards(cards);
        repo.CreateNewStack(flashcardStackToCreate);

        flashcardStackToCreate.Subject = "English";
        
        Assert.Multiple(() =>
        {
            Assert.That(repo.UpdateStack(flashcardStackToCreate), Is.True);
            Assert.That(repo.ReadStackById(flashcardStackToCreate.Id)!.Subject, Is.EqualTo("English"));
        });
    }

    [Test]
    public void UpdateStackWithNewCards()
    {
        FlashcardStack flashcardStackToCreate = new("French");
        
        List <Flashcard> cards =
        [
            new Flashcard("Dog", "Chien", flashcardStackToCreate.Id),
            new Flashcard("Cat", "Chat", flashcardStackToCreate.Id)
        ];

        flashcardStackToCreate.AddCards(cards);
        repo.CreateNewStack(flashcardStackToCreate);
        
        flashcardStackToCreate.AddCard(new Flashcard("Mouse", "Souris", flashcardStackToCreate.Id));
        
        Assert.Multiple(() =>
        {
            Assert.That(repo.UpdateStack(flashcardStackToCreate), Is.True);
            Assert.That(repo.ReadStackById(flashcardStackToCreate.Id)!.GetCards(),
                Has.Count.EqualTo(3));
        });
    }

    [Test]
    public void DeleteStack()
    {
        FlashcardStack flashcardStackToCreate = new("French");
        
        List <Flashcard> cards =
        [
            new Flashcard("Dog", "Chien", flashcardStackToCreate.Id),
            new Flashcard("Cat", "Chat", flashcardStackToCreate.Id)
        ];

        flashcardStackToCreate.AddCards(cards);
        repo.CreateNewStack(flashcardStackToCreate);

        repo.DeleteStack(flashcardStackToCreate);
        
        Assert.That(repo.ReadAllStacks().ToList(), Has.Count.EqualTo(0));
    }
}