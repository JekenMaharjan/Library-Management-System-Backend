
using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }


        // ===============================
        // POST: api/Students
        // Purpose: Add a new student
        // ===============================
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }


        // ===============================
        // GET: api/Students
        // GET: api/Students?rollNo=ABC123
        // Purpose: Retrieve all students or filter by roll number
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] string? rollNo)
        {
            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(rollNo))
            {
                students = students.Where(s => s.RollNo == rollNo);
            }

            return Ok(await students.ToListAsync());
        }


        // ===============================
        // GET: api/Students/{id}
        // Purpose: Retrieve a specific student by ID
        // ===============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }


        // ===============================
        // PUT: api/Students/{id}
        // Purpose: Update student details
        // ===============================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student updatedStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found");

            student.Name = updatedStudent.Name;
            student.RollNo = updatedStudent.RollNo;

            await _context.SaveChangesAsync();
            return Ok(student);
        }


        // ===============================
        // DELETE: api/Students/{id}
        // Purpose: Delete a student
        // ===============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Student deleted successfully" });
        }


        // ===============================
        // GET: api/Students/total
        // Purpose: Get total number of students
        // ===============================
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalStudents()
        {
            var totalStudents = await _context.Students.CountAsync();
            return Ok(new { totalStudents });
        }
    }
}
