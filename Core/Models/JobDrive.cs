namespace PlacementManagementSystem.Models;

public class JobDrive
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Role { get; set; } = "";
    public double PackageLpa { get; set; }
    public double MinCgpa { get; set; }
    public DateTime DriveDate { get; set; }
}
