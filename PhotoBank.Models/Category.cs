using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType("Text",ErrorMessage = "Enter category name")]
        public string Name { get; set; }

    }
}
