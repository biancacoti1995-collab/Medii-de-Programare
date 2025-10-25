using System.ComponentModel.DataAnnotations;

namespace Coti_Bianca_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public ICollection<Book>? Books { get; set; }

        public string FullName => $"{LastName} {FirstName}";
    }
}
