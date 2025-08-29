using Dapper;
using Flashcards.mosnyan.Application.Abstractions.Repositories;
using Flashcards.mosnyan.Domain.Models;
using MySqlConnector;

namespace Flashcards.mosnyan.Infrastructure.Repository;

public class SessionRepository(string connectionString) : ISessionRepository
{
    public bool CreateNewSession(Session session)
    {
        using var connection = new MySqlConnection(connectionString);

        var query = "INSERT INTO history (id, t_stamp, score, stack_id) " +
                    "VALUES (@Id, @TimeStamp, @Score, @StackId)";

        try
        {
            return connection.Execute(query, session) > 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public IEnumerable<Session> ReadAllSessions()
    {
        using var connection = new MySqlConnection(connectionString);

        var query = "SELECT * FROM history";

        return connection.Query(query)
            .Select(row => new Session(row.id, row.t_stamp, row.score, row.stack_id));
    }
}