using MySqlConnector;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Data;

public class JobDriveRow
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = "";
    public string Role { get; set; } = "";
    public double PackageLpa { get; set; }
    public double MinCgpa { get; set; }
    public DateTime DriveDate { get; set; }
    public string Display => $"{CompanyName} - {Role} (min CGPA {MinCgpa})";
}

public class JobDriveRepository
{
    public int Add(JobDrive j)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "INSERT INTO job_drives (company_id, role, package_lpa, min_cgpa, drive_date) " +
            "VALUES (@companyId, @role, @package, @minCgpa, @date); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@companyId", j.CompanyId);
        cmd.Parameters.AddWithValue("@role", j.Role);
        cmd.Parameters.AddWithValue("@package", j.PackageLpa);
        cmd.Parameters.AddWithValue("@minCgpa", j.MinCgpa);
        cmd.Parameters.AddWithValue("@date", j.DriveDate.Date);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<JobDrive> GetAll()
    {
        var list = new List<JobDrive>();
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "SELECT id, company_id, role, package_lpa, min_cgpa, drive_date FROM job_drives ORDER BY id", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new JobDrive
            {
                Id = reader.GetInt32("id"),
                CompanyId = reader.GetInt32("company_id"),
                Role = reader.GetString("role"),
                PackageLpa = reader.GetDouble("package_lpa"),
                MinCgpa = reader.GetDouble("min_cgpa"),
                DriveDate = reader.GetDateTime("drive_date")
            });
        }
        return list;
    }

    public List<JobDriveRow> GetAllWithCompanyName()
    {
        var list = new List<JobDriveRow>();
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(@"
            SELECT j.id, c.name AS company_name, j.role, j.package_lpa, j.min_cgpa, j.drive_date
            FROM job_drives j
            JOIN companies c ON c.id = j.company_id
            ORDER BY j.id", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new JobDriveRow
            {
                Id = reader.GetInt32("id"),
                CompanyName = reader.GetString("company_name"),
                Role = reader.GetString("role"),
                PackageLpa = reader.GetDouble("package_lpa"),
                MinCgpa = reader.GetDouble("min_cgpa"),
                DriveDate = reader.GetDateTime("drive_date")
            });
        }
        return list;
    }

    public JobDrive? GetById(int id)
    {
        using var conn = DbConfig.GetConnection();
        conn.Open();
        using var cmd = new MySqlCommand(
            "SELECT id, company_id, role, package_lpa, min_cgpa, drive_date FROM job_drives WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new JobDrive
        {
            Id = reader.GetInt32("id"),
            CompanyId = reader.GetInt32("company_id"),
            Role = reader.GetString("role"),
            PackageLpa = reader.GetDouble("package_lpa"),
            MinCgpa = reader.GetDouble("min_cgpa"),
            DriveDate = reader.GetDateTime("drive_date")
        };
    }
}
