using MySqlConnector;

namespace Flashcards.mosnyan.Infrastructure.Persistence;

public class Initializer(string initialConnString, string databaseName)
{
    private string ConnectionString { get; } = $"Server=localhost;Port=3306;Database={databaseName};Uid=user;Pwd=password;";
    public void Initialize()
    {
        CreateDatabase();
        CreateStacksTable();
        CreateCardsTable();
        CreateHistoryTable();
    }

    public bool DoesDatabaseExists()
    {
        using var connection = new MySqlConnection(initialConnString);

        connection.Open();
        var query = $"SHOW DATABASES LIKE '{databaseName}';";
        var command = new MySqlCommand(query, connection);
        var result = command.ExecuteScalar();

        return true;
    }

    private void CreateDatabase()
    {
        using var connection = new MySqlConnection(initialConnString);
        
        connection.Open();
        var query = $"CREATE DATABASE IF NOT EXISTS {databaseName};";
        var command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateStacksTable()
    {
        using var connection = new MySqlConnection(ConnectionString);

        connection.Open();
        var query = "CREATE TABLE IF NOT EXISTS stacks" +
                    " (" +
                    "id UUID PRIMARY KEY," +
                    "subject TEXT NOT NULL" +
                    ");";
        var command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateCardsTable()
    {
        using var connection = new MySqlConnection(ConnectionString);
        
        connection.Open();
        var query = "CREATE TABLE IF NOT EXISTS cards" +
                    " (" +
                    "id UUID PRIMARY KEY," +
                    "prompt TEXT NOT NULL," +
                    "answer TEXT NOT NULL," +
                    "stack_id UUID NOT NULL," +
                    "FOREIGN KEY (stack_id) REFERENCES stacks(id) ON DELETE CASCADE" +
                    ");";
        var command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private void CreateHistoryTable()
    {
        using var connection = new MySqlConnection(ConnectionString);
        
        connection.Open();
        var query = "CREATE TABLE IF NOT EXISTS history" +
                    " (" +
                    "id UUID PRIMARY KEY," +
                    "t_stamp DATETIME NOT NULL," +
                    "score FLOAT NOT NULL," +
                    "stack_id UUID NOT NULL," +
                    "FOREIGN KEY (stack_id) REFERENCES stacks(id) ON DELETE CASCADE" +
                    ");";
        var command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }
}