namespace Models.Models;

public class Student
{
    public int StudentId { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public string FullName => $"{LastName}, {FirstMidName}";
}
