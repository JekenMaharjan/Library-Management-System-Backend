using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }


        // ===============================
        // POST: api/Books
        // Add a new book
        // ===============================
        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }


        // ===============================
        // GET: api/Books
        // GET: api/Books?title=xyz
        // Get all books or search by title
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetBooks(string? title)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                books = books.Where(b =>
                    EF.Functions.Like(b.Title, $"%{title}%")
                );
            }

            return Ok(await books.ToListAsync());
        }


        // ===============================
        // PUT: api/Books/{id}
        // Update book details
        // ===============================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound("Book not found");

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.TotalStock = updatedBook.TotalStock;

            await _context.SaveChangesAsync();
            return Ok(book);
        }


        // ===============================
        // DELETE: api/Books/{id}
        // Delete a book
        // ===============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound("Book not found");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok("Book deleted successfully");
        }


        // ========================================================


        // ===============================
        // GET: api/Books/stats
        // Get book statistics
        // ===============================
        [HttpGet("stats")]
        public async Task<IActionResult> GetBookStats()
        {
            // Total physical books in library
            var totalBooks = await _context.Books
            .SumAsync(b => b.TotalStock);

            // Total issued / reserved books (not yet returned)
            var issuedBooks = await _context.BookIssues
            .CountAsync(i => i.ReturnDate == null);

            // Total available books in library
            var availableBooks = await _context.Books
                .Where(b => b.TotalStock > 0)
                .SumAsync(b => b.TotalStock);

            return Ok(new
            {
                totalBooks,
                issuedBooks,
                availableBooks
            });
        }
    }
}
