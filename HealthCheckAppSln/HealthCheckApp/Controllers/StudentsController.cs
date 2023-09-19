using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DataAccess;
using Models.Models;

namespace StudenApp.Controllers;
[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentAppContext _context;

    public StudentsController(StudentAppContext context)
    {
        _context = context;
    }

    // GET: Students

    [HttpGet("GetStudents")]
    public async Task<IActionResult> GetStudents()
    {
        return _context.Students != null ?
                    Ok(await _context.Students.ToListAsync()) :
                    Problem("Entity set 'StudentDemoContext.Students'  is null.");
    }

    [HttpGet("GetStudent/{id}")]
    public async Task<IActionResult> GetStudent(int? id)
    {
        if (id == null || _context.Students == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }



    // POST: Students/Create 
    [HttpPost("SaveStudent")]
    public async Task<IActionResult> SaveStudent(Student student)
    {
        if (ModelState.IsValid)
        {
            if (!StudentExists(student.StudentId))
            {
                _context.Add(student);
            }
            else
            {
                try
                {
                    _context.Update(student);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        return Ok(student);
    }



    [HttpDelete("DeleteStudent")]
    public async Task<IActionResult> Delete(int id)
    {
        if (_context.Students == null)
        {
            return Problem("Entity set 'StudentDemoContext.Students'  is null.");
        }
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
        }

        await _context.SaveChangesAsync();
        return Ok("Saved");
    }

    private bool StudentExists(int id)
    {
        return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
    }
}