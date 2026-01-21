using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IssuesController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET: api/Issues
        // Purpose: Retrieve all issued books with related book and student details
        // ===============================
        [HttpGet]
        public IActionResult GetAllIssues()
        {
            var issues = _context.BookIssues
        .Include(i => i.Book)
        .Include(i => i.Student)
        .Select(i => new
        {
            i.IssueId,
            BookTitle = i.Book.Title,
            StudentName = i.Student.Name,
            i.IssueDate,
            i.ReturnDate,
            i.IsReturned
        })
        .ToList();

            return Ok(issues);
        }

        // ===============================
        // POST: api/Issues/issue
        // Purpose: Issue a book to a student if stock is available
        // ===============================
        [HttpPost("issue")]
        public IActionResult IssueBook(int bookId, int studentId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null || book.TotalStock <= 0)
                return BadRequest("Book not available");

            // Reduce stock when book is issued
            book.TotalStock--;

            var issue = new BookIssue
            {
                BookId = bookId,
                StudentId = studentId,
                IssueDate = DateTime.Now,
                IsReturned = false
            };

            _context.BookIssues.Add(issue);
            _context.SaveChanges();

            return Ok("Book issued successfully");
        }

        // ===============================
        // POST: api/Issues/return
        // Purpose: Return an issued book and update stock
        // ===============================
        [HttpPost("return")]
        public IActionResult ReturnBook(int issueId)
        {
            var issue = _context.BookIssues.Find(issueId);
            if (issue == null || issue.IsReturned)
                return BadRequest("Invalid request");

            // Mark book as returned
            issue.IsReturned = true;
            issue.ReturnDate = DateTime.Now;

            // Increase stock after return
            var book = _context.Books.Find(issue.BookId);
            book.TotalStock++;

            _context.SaveChanges();
            return Ok("Book returned successfully");
        }

    }
}
