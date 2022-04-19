using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PhotoBank.Models
{
    public class User : IdentityUser
    {
        [Required]
        [EmailAddress]
        //  [DataType("Email",ErrorMessage = "Enter correct Email address")]
        public string Email { get; set; }
        [Required]
        [DataType("Text", ErrorMessage = "Enter correct first name")]
        public string FirstName { get; set; }
        [Required]
        [DataType("Text", ErrorMessage = "Enter correct Last name")]
        public string LastName { get; set; }
        public string? Address { get; set; }
        //[DataType("PhoneNumber",ErrorMessage = "Enter correct phone number")]
        [Phone]
        public string? Phone { get; set; }
        public bool IsAuthor { get; set; }

        public User()
        {
            Phone = "+380";
        }

    }
}
