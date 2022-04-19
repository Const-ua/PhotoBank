using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoBank.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("PhotoId")]
        public Photo Photo { get; set; }
    }
}
