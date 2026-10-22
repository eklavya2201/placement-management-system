namespace PlacementManagementSystem.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Branch { get; set; } = "";
    public int GraduationYear { get; set; }
    public double Cgpa { get; set; }
    public string Email { get; set; } = "";
}
