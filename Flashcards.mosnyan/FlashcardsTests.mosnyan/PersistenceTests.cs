using Flashcards.mosnyan.Infrastructure.Persistence;

namespace FlashcardsTests.mosnyan;

public class Tests
{
    [Test]
    public void DatabaseIsCreated()
    {
        var str = "Server=localhost;Port=3306;Uid=user;Pwd=password;";
        var initializer = new Initializer(str, "flashcards_tests");
        initializer.Initialize();
        
        Assert.That(initializer.DoesDatabaseExists(), Is.True);
    }
}