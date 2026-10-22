using MySqlConnector;

namespace PlacementManagementSystem.Data;

public class TableData
{
    public string Name { get; set; } = "";
    public List<string> Columns { get; } = new();
    public List<string[]> Rows { get; } = new();
}

// Read-only view of the raw MySQL tables, for showing the database itself.
public class DatabaseRepository
{
    public static readonly string[] Tables = { "students", "companies", "job_drives", "applications" };

    public (string Version, string Database) GetServerInfo()
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("SELECT VERSION(), DATABASE()", conn);
        using var reader = cmd.ExecuteReader();
        reader.Read();
        return (reader.GetString(0), reader.GetString(1));
    }

    public TableData GetTable(string name)
    {
        if (!Tables.Contains(name)) throw new ArgumentException("Unknown table", nameof(name));

        var table = new TableData { Name = name };
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand($"SELECT * FROM `{name}` ORDER BY id", conn);
        using var reader = cmd.ExecuteReader();
        for (var i = 0; i < reader.FieldCount; i++)
            table.Columns.Add(reader.GetName(i));
        while (reader.Read())
        {
            var row = new string[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
                row[i] = reader.GetValue(i) is DateTime d ? d.ToString("yyyy-MM-dd") : reader.GetValue(i).ToString() ?? "";
            table.Rows.Add(row);
        }
        return table;
    }
}
