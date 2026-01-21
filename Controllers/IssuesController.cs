using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("issue")]
        public IActionResult IssueBook(int bookId, int studentId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null || book.TotalStock <= 0)
                return BadRequest("Book not available");

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

        [HttpPost("return")]
        public IActionResult ReturnBook(int issueId)
        {
            var issue = _context.BookIssues.Find(issueId);
            if (issue == null || issue.IsReturned)
                return BadRequest("Invalid request");

            issue.IsReturned = true;
            issue.ReturnDate = DateTime.Now;

            var book = _context.Books.Find(issue.BookId);
            book.TotalStock++;

            _context.SaveChanges();
            return Ok("Book returned successfully");
        }
    }
}
