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
        // POST: api/Issues/issue
        // Issue a book to a student
        // ===============================
        [HttpPost("issue")]
        public async Task<IActionResult> IssueBook(int bookId, int studentId)
        {
            var book = await _context.Books.FindAsync(bookId);

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
            await _context.SaveChangesAsync();

            return Ok("Book issued successfully");
        }

        // ===============================
        // POST: api/Issues/return
        // Return a book
        // ===============================
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int issueId)
        {
            var issue = await _context.BookIssues.FindAsync(issueId);

            if (issue == null || issue.IsReturned)
                return BadRequest("Invalid request");

            issue.IsReturned = true;
            issue.ReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(issue.BookId);

            if (book == null)
                return BadRequest("Associated book not found");

            book.TotalStock++;

            await _context.SaveChangesAsync();
            return Ok("Book returned successfully");
        }

        // ===============================
        // GET: api/Issues
        // GET: api/Issues?title=xyz
        // Get all issues or search by book title
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetAllIssues([FromQuery] string? title)
        {
            var issues = _context.BookIssues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .AsQueryable();

            // Filter by book title if title is provided
            if (!string.IsNullOrWhiteSpace(title))
            {
                issues = issues.Where(i =>
                    EF.Functions.Like(i.Book.Title, $"%{title}%")
                );
            }

            // Project the result
            var result = await issues.Select(i => new
            {
                issueId = i.IssueId,
                bookTitle = i.Book.Title,
                studentName = i.Student.Name,
                issueDate = i.IssueDate,
                returnDate = i.ReturnDate,
                isReturned = i.IsReturned
            }).ToListAsync();

            return Ok(result);
        }

    }
}
