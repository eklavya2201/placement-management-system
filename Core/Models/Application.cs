namespace PlacementManagementSystem.Models;

public enum ApplicationStatus
{
    Applied,
    Shortlisted,
    Selected,
    Rejected
}

public class Application
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int JobDriveId { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
}
