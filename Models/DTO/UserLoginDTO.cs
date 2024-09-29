using System;
using System.ComponentModel.DataAnnotations;

namespace ArtistPortfolio.Models.DTO
{
	public class UserLoginDTO
	{
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        [Required(ErrorMessage = "Email address is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

