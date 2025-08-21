using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.mosnyan.Domain.Models;
using Flashcards.mosnyan.Infrastructure.Repository;

namespace FlashcardsTests.mosnyan;

public class RepositoryTests
{
    private const String ConnectionString = $"Data Source=localhost;" +
                                            $"Initial Catalog=flashcards_tests;" +
                                            $"Integrated Security=True;" +
                                            $"Encrypt=False;";

    private StackRepository repo = new(ConnectionString);
    
    [SetUp]
    public void SetUp()
    {
        using var connection = new SqlConnection(ConnectionString);
        var query = "DELETE FROM stacks;";
        connection.Execute(query);
    }

    [Test]
    public void CreateStack()
    {
        Stack stack = new("French", []);
        Assert.That(repo.CreateNewStack(stack), Is.True);
    }

    [Test]
    public void CreateStackWithCards()
    {
        Stack stack = new("French");
        
        List <Card> cards =
        [
            new Card("Dog", "Chien", stack.Id),
            new Card("Cat", "Chat", stack.Id)
        ];

        stack = stack.AddCards(cards);
        
        Assert.That(repo.CreateNewStack(stack), Is.True);
    }

    [Test]
    public void GetAllStacks()
    {
        Stack stackToCreate = new("French", []);
        repo.CreateNewStack(stackToCreate);

        var stacks = repo.ReadAllStacks().ToList();
        
        Assert.That(stacks, Has.Count.EqualTo(1));

        var resultStack = stacks.ElementAt(0);
        Assert.That(resultStack.Id, Is.EqualTo(stackToCreate.Id));
        Assert.That(resultStack.Subject, Is.EqualTo(stackToCreate.Subject));
        Assert.That(resultStack.GetCards().ToList(), Has.Count.EqualTo(0));
    }
    
    [Test]
    public void GetAllStacksWithCards()
    {
        Stack stackToCreate = new("French");
        
        List <Card> cards =
        [
            new Card("Dog", "Chien", stackToCreate.Id),
            new Card("Cat", "Chat", stackToCreate.Id)
        ];

        stackToCreate = stackToCreate.AddCards(cards);
        repo.CreateNewStack(stackToCreate);

        var stacks = repo.ReadAllStacks().ToList();
        
        Assert.That(stacks, Has.Count.EqualTo(1));

        var resultStack = stacks.ElementAt(0);
        Assert.That(resultStack.Id, Is.EqualTo(stackToCreate.Id));
        Assert.That(resultStack.Subject, Is.EqualTo(stackToCreate.Subject));
        Assert.That(resultStack.GetCards().ToList(), Has.Count.EqualTo(2));
    }
}