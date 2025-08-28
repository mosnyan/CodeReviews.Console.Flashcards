using Dapper;
using Flashcards.mosnyan.Application.Abstractions.Repositories;
using Flashcards.mosnyan.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.mosnyan.Infrastructure.Repository;

public class StackRepository(string connectionString) : IStackRepository
{
    public bool CreateNewStack(FlashcardStack flashcardStack)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var query = "INSERT INTO stacks (id, subject) " +
                              "VALUES (@Id, @Subject)";
            connection.Execute(query, flashcardStack, transaction);
            
            query = "INSERT INTO cards (id, prompt, answer, stack_id) " +
                    "VALUES (@Id, @Prompt, @Answer, @StackId)";
            connection.Execute(query, flashcardStack.GetCards(), transaction);
            
            transaction.Commit();

            return true;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return false;
        }
    }

    public IEnumerable<FlashcardStack> ReadAllStacks()
    {
        using var connection = new SqlConnection(connectionString);

        var query = "SELECT s.id, s.subject, c.id AS cardId, c.prompt, c.answer, c.stack_id AS stackId " +
                    "FROM stacks AS s " +
                    "LEFT JOIN cards AS c ON s.id = c.stack_id";

        var stackDict = new Dictionary<Guid, FlashcardStack>();

        connection.Query<FlashcardStack, Flashcard, FlashcardStack>(
            query, (stack, card) =>
            {
                if (!stackDict.TryGetValue(stack.Id, out var currentStack))
                {
                    currentStack = stack;
                    stackDict.Add(currentStack.Id, currentStack);
                }

                currentStack.AddCard(card);

                return currentStack;
            }, splitOn: "cardId"
        );

        return stackDict.Values;
    }

    public FlashcardStack? ReadStackById(Guid id)
    {
        using var connection = new SqlConnection(connectionString);

        var query = "SELECT s.id, s.subject, c.id AS cardId, c.prompt, c.answer, c.stack_id AS stackId " +
                    "FROM stacks AS s " +
                    "LEFT JOIN cards AS c ON s.id = c.stack_id " +
                    "WHERE s.id = @id";
        
        var stackDict = new Dictionary<Guid, FlashcardStack>();
        
        connection.Query<FlashcardStack, Flashcard, FlashcardStack>(
            query, (stack, card) =>
            {
                if (!stackDict.TryGetValue(stack.Id, out var currentStack))
                {
                    currentStack = stack;
                    stackDict.Add(currentStack.Id, currentStack);
                }

                currentStack.AddCard(card);

                return currentStack;
            }, splitOn: "cardId", param: new { id = id }
        );

        if (stackDict.IsNullOrEmpty())
        {
            return null;
        }

        if (stackDict.Count > 1)
        {
            throw new InvalidOperationException($"Primary key violation for ID {id}.");
        }

        return stackDict.Values.Single();
    }

    public bool UpdateStack(FlashcardStack flashcardStack)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var query = "UPDATE stacks " +
                        "SET subject = @Subject " +
                        "WHERE id = @Id";
            connection.Execute(query, flashcardStack, transaction);

            query = "UPDATE cards " +
                    "SET prompt = @Prompt, " +
                    "answer = @Answer " +
                    "WHERE id = @Id";
            connection.Execute(query, flashcardStack.GetCards(), transaction);

            query = "IF NOT EXISTS (SELECT 1 FROM cards WHERE id = @Id) " +
                    "BEGIN " +
                    "INSERT INTO cards (id, prompt, answer, stack_id) " +
                    "VALUES (@Id, @Prompt, @Answer, @StackId) " +
                    "END";
            
            connection.Execute(query, flashcardStack.GetCards(), transaction);
            
            transaction.Commit();
            
            return true;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return false;
        }
    }

    public bool DeleteStack(FlashcardStack flashcardStack)
    {
        using var connection = new SqlConnection(connectionString);

        var query = "DELETE FROM stacks WHERE id = @Id";
        
        return connection.Execute(query, flashcardStack) > 0;
    }

    public bool DeleteStackById(Guid id)
    {
        using var connection = new SqlConnection(connectionString);

        var query = $"DELETE FROM stacks WHERE id = {id}";
        
        return connection.Execute(query) > 0;
    }
}