using Dapper;
using Flashcards.mosnyan.Application.Abstractions.Repositories;
using Flashcards.mosnyan.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.mosnyan.Infrastructure.Repository;

public class StackRepository(string connectionString) : IStackRepository
{
    public bool CreateNewStack(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var query = "INSERT INTO stacks (id, subject) " +
                              "VALUES (@Id, @Subject)";
            connection.Execute(query, stack, transaction);
            
            query = "INSERT INTO cards (id, prompt, answer, stack_id) " +
                    "VALUES (@Id, @Prompt, @Answer, @StackId)";
            connection.Execute(query, stack.GetCards(), transaction);
            
            transaction.Commit();

            return true;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return false;
        }
    }

    public IEnumerable<Stack> ReadAllStacks()
    {
        using var connection = new SqlConnection(connectionString);

        var query = "SELECT * FROM stacks";
        var stackRows = connection.Query(query);

        query = "SELECT * FROM cards";
        var cardRows = connection.Query(query);

        return stackRows.Select(stack => new Stack(
            stack.id,
            stack.subject,
            cardRows.Where(card => card.stack_id == stack.id)
                .Select(card => new Card(
                    card.id,
                    card.prompt,
                    card.answer,
                    card.stack_id))
                .ToList()
        ));
    }

    public Stack? ReadStackById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool UpdateStack(Stack stack)
    {
        throw new NotImplementedException();
    }

    public bool DeleteStack(Stack stack)
    {
        throw new NotImplementedException();
    }

    public bool DeleteStackById(Guid id)
    {
        throw new NotImplementedException();
    }
}