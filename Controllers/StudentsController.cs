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

        // GET: api/students
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return Ok(student);
        }

        // PUT: api/students/5
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

        // DELETE: api/students/5
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
