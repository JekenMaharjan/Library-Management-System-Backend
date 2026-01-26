using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models
{
    public class BookIssue
    {
        [Key]
        public int IssueId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; 

        public DateTime IssueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        public Book Book { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
