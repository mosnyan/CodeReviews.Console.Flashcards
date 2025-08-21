using Microsoft.Data.SqlClient;

namespace Flashcards.mosnyan.Infrastructure.Persistence;

public class Initializer(string initialConnString, string databaseName)
{
    private string ConnectionString { get; } = $"Data Source=localhost;" +
                                               $"Initial Catalog={databaseName};" +
                                               $"Integrated Security=True;" +
                                               $"Encrypt=False;";
    public void Initialize()
    {
        CreateDatabase();
        CreateStacksTable();
        CreateCardsTable();
        CreateHistoryTable();
    }

    public bool DoesDatabaseExists()
    {
        using var connection = new SqlConnection(initialConnString);

        connection.Open();
        var query = $"SELECT database_id FROM sys.databases WHERE name = '{databaseName}'";
        var command = new SqlCommand(query, connection);
        var result = command.ExecuteScalar();

        return (int)result > 0;
    }

    private void CreateDatabase()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        connection.Open();
        var query = $"IF (DB_ID('{databaseName}') IS NULL) " +
                    $"CREATE DATABASE [{databaseName}];";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateStacksTable()
    {
        using var connection = new SqlConnection(ConnectionString);

        connection.Open();
        var query = "IF (OBJECT_ID(N'stacks', N'U') IS NULL) " +
                    "CREATE TABLE stacks" +
                    " (" +
                    "id uniqueidentifier PRIMARY KEY," +
                    "subject text NOT NULL" +
                    ");";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateCardsTable()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        connection.Open();
        var query = "IF (OBJECT_ID(N'cards', N'U') IS NULL) " +
                    "CREATE TABLE cards" +
                    " (" +
                    "id uniqueidentifier PRIMARY KEY," +
                    "prompt text NOT NULL," +
                    "answer text NOT NULL," +
                    "stack_id uniqueidentifier NOT NULL," +
                    "FOREIGN KEY (stack_id) REFERENCES stacks(id) ON DELETE CASCADE" +
                    ");";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateHistoryTable()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        connection.Open();
        var query = "IF (OBJECT_ID(N'history', N'U') IS NULL) " +
                    "CREATE TABLE history" +
                    " (" +
                    "id uniqueidentifier PRIMARY KEY," +
                    "t_stamp datetime NOT NULL," +
                    "score float NOT NULL," +
                    "stack_id uniqueidentifier NOT NULL," +
                    "FOREIGN KEY (stack_id) REFERENCES stacks(id) ON DELETE CASCADE" +
                    ");";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
    }
}