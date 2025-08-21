using Flashcards.mosnyan.Infrastructure.Persistence;

namespace FlashcardsTests.mosnyan;

public class Tests
{
    [Test]
    public void DatabaseIsCreated()
    {
        var str = "Data Source=localhost;Initial Catalog=flashcards_tests;Integrated Security=True;Encrypt=False;";
        var initializer = new Initializer(str, "flashcards_tests");
        initializer.Initialize();
        
        Assert.That(initializer.DoesDatabaseExists(), Is.True);
    }
}