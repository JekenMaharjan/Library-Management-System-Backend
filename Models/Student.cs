using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public string Name { get; set; }

        public string RollNo { get; set; }
    }
}
