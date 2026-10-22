using MySqlConnector;

namespace PlacementManagementSystem.Data;

public static class DbConfig
{
    // Set DB_HOST / DB_PORT / DB_NAME / DB_USER / DB_PASSWORD as environment variables
    // (done on Render), or edit the fallback values below for a local MySQL.
    public static string Server { get; set; } = Env("DB_HOST", "localhost");
    public static string Database { get; set; } = Env("DB_NAME", "placement_db");
    public static string User { get; set; } = Env("DB_USER", "root");
    public static string Password { get; set; } = Env("DB_PASSWORD", "your_password");
    public static uint Port { get; set; } = uint.Parse(Env("DB_PORT", "3306"));

    private static string Env(string name, string fallback) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value : fallback;

    public static MySqlConnection GetConnection()
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = Server,
            Database = Database,
            UserID = User,
            Password = Password,
            Port = Port
        };
        return new MySqlConnection(builder.ConnectionString);
    }
}
