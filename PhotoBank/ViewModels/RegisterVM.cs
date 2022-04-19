using System.ComponentModel.DataAnnotations;

namespace PhotoBank.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter correct Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get;set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password don't match!")]
        [Compare("Password")]
        public string ConfirmPassword { get;set; }
        [Required]
        public string FirstName { get;set; }
        [Required]
        public string LastName { get;set; }
        //[Required]
        public string Phone { get;set; }
        //[Required]
        public string Address { get; set; }
        public bool IsAuthor { get; set; }



    }
}
