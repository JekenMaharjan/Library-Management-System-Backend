using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

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
        // GET: api/Books
        // Purpose: Retrieve all books available in the library
        // ===============================
        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(_context.Books.ToList());
        }

        // ===============================
        // POST: api/Books
        // Purpose: Add a new book to the library inventory
        // ===============================
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok(book);
        }

        // ===============================
        // PUT: api/Books/{id}
        // Purpose: Update book details such as title, author, and stock
        // ===============================
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            // Update fields (explicit & safe)
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.TotalStock = updatedBook.TotalStock;

            _context.SaveChanges();
            return Ok(book);
        }

        // ===============================
        // DELETE: api/Books/{id}
        // Purpose: Remove a book from the library system
        // ===============================
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return Ok("Book deleted successfully");
        }
    }
}
