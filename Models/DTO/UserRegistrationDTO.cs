using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtistPortfolio.Models.DTO
{
	public class UserRegistrationDTO
	{
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100)]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        [Required(ErrorMessage = "Email address is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        // Add a property for selected role
        public string SelectedRole { get; set; }

        // Add a list of available roles
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}

