using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Models.DataAccess;
public class StudentAppContext : DbContext
{
    public StudentAppContext(DbContextOptions<StudentAppContext> options) : base(options)
    {
    }


    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
    }
}
