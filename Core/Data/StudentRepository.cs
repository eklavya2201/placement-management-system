using MySqlConnector;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Data;

public class StudentRepository
{
    public int Add(Student s)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "INSERT INTO students (name, branch, graduation_year, cgpa, email) " +
            "VALUES (@name, @branch, @year, @cgpa, @email); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@name", s.Name);
        cmd.Parameters.AddWithValue("@branch", s.Branch);
        cmd.Parameters.AddWithValue("@year", s.GraduationYear);
        cmd.Parameters.AddWithValue("@cgpa", s.Cgpa);
        cmd.Parameters.AddWithValue("@email", s.Email);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Student> GetAll()
    {
        var list = new List<Student>();
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id, name, branch, graduation_year, cgpa, email FROM students ORDER BY id", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Student
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Branch = reader.GetString("branch"),
                GraduationYear = reader.GetInt32("graduation_year"),
                Cgpa = reader.GetDouble("cgpa"),
                Email = reader.GetString("email")
            });
        }
        return list;
    }

    public Student? GetById(int id)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id, name, branch, graduation_year, cgpa, email FROM students WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new Student
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Branch = reader.GetString("branch"),
            GraduationYear = reader.GetInt32("graduation_year"),
            Cgpa = reader.GetDouble("cgpa"),
            Email = reader.GetString("email")
        };
    }
}
