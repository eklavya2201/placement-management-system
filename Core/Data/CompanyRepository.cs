using MySqlConnector;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Data;

public class CompanyRepository
{
    public int Add(Company c)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "INSERT INTO companies (name, sector, contact_email) VALUES (@name, @sector, @email); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@name", c.Name);
        cmd.Parameters.AddWithValue("@sector", c.Sector);
        cmd.Parameters.AddWithValue("@email", c.ContactEmail);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Company> GetAll()
    {
        var list = new List<Company>();
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id, name, sector, contact_email FROM companies ORDER BY id", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Company
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Sector = reader.GetString("sector"),
                ContactEmail = reader.GetString("contact_email")
            });
        }
        return list;
    }

    public Company? GetById(int id)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id, name, sector, contact_email FROM companies WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new Company
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Sector = reader.GetString("sector"),
            ContactEmail = reader.GetString("contact_email")
        };
    }
}
