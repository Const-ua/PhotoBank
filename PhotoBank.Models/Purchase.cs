using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id { get; set; }
        public string PhotoId { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

    }
}
