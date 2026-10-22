using MySqlConnector;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Data;

public class ApplicationRow
{
    public int Id { get; set; }
    public string StudentName { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string Role { get; set; } = "";
    public string Status { get; set; } = "";
}

public class ApplicationRepository
{
    public int Add(Application a)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "INSERT INTO applications (student_id, job_drive_id, status) VALUES (@studentId, @jobDriveId, @status); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@studentId", a.StudentId);
        cmd.Parameters.AddWithValue("@jobDriveId", a.JobDriveId);
        cmd.Parameters.AddWithValue("@status", a.Status.ToString());
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public bool Exists(int studentId, int jobDriveId)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "SELECT COUNT(*) FROM applications WHERE student_id = @studentId AND job_drive_id = @jobDriveId", conn);
        cmd.Parameters.AddWithValue("@studentId", studentId);
        cmd.Parameters.AddWithValue("@jobDriveId", jobDriveId);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public List<ApplicationRow> GetAllWithDetails()
    {
        var list = new List<ApplicationRow>();
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(@"
            SELECT a.id, s.name AS student_name, c.name AS company_name, j.role, a.status
            FROM applications a
            JOIN students s ON s.id = a.student_id
            JOIN job_drives j ON j.id = a.job_drive_id
            JOIN companies c ON c.id = j.company_id
            ORDER BY a.id", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new ApplicationRow
            {
                Id = reader.GetInt32("id"),
                StudentName = reader.GetString("student_name"),
                CompanyName = reader.GetString("company_name"),
                Role = reader.GetString("role"),
                Status = reader.GetString("status")
            });
        }
        return list;
    }

    public bool UpdateStatus(int applicationId, ApplicationStatus status)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand("UPDATE applications SET status = @status WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@status", status.ToString());
        cmd.Parameters.AddWithValue("@id", applicationId);
        return cmd.ExecuteNonQuery() > 0;
    }
}
