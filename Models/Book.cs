using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public int TotalStock { get; set; }
    }
}
