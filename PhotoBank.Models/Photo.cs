using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoBank.Models
{
    public class Photo
    {

        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string File { get; set; }
        public string Preview { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }
        public DateTime Published { get; set; }
        [Required]
        [Range(0.01,Double.MaxValue, ErrorMessage="Price must be grater then 0.00")]
        public double Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
       
    }
}
