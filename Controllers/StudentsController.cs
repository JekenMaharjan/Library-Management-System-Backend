using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

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
        // GET: api/Students
        // Purpose: Retrieve all students registered in the library system
        // ===============================
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        // ===============================
        // GET: api/Students/{id}
        // Purpose: Retrieve a specific student by ID
        // ===============================
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }

        // ===============================
        // POST: api/Students
        // Purpose: Add a new student to the library system
        // ===============================a
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return Ok(student);
        }

        // ===============================
        // PUT: api/Students/{id}
        // Purpose: Update student details such as name and roll number
        // ===============================
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student updatedStudent)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound("Student not found");

            student.Name = updatedStudent.Name;
            student.RollNo = updatedStudent.RollNo;

            _context.SaveChanges();
            return Ok(student);
        }

        // ===============================
        // DELETE: api/Students/{id}
        // Purpose: Remove a student from the library system
        // ===============================
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound("Student not found");

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok("Student deleted successfully");
        }
    }
}
