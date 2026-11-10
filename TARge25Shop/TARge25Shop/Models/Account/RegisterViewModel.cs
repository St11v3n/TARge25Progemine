using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confrimation password do not match.")]

        public string ConfirmPassword { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
    }
}
