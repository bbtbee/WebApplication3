using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
	public class Register
	{
		[Required(ErrorMessage = "Please enter your email address.")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address format.")]
		public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*\(\)_\+{}\[\]:;<>,.?~\\/\-])[A-Za-z\d!@#\$%\^&\*\(\)_\+{}\[\]:;<>,.?~\\/\-]+$",
           ErrorMessage = "Password must meet complexity requirements.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please confirm your password.")]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Please enter your first name.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Please enter your last name.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Please select a gender.")]
		public string Gender { get; set; }

		[Required(ErrorMessage = "Please enter a valid NRIC.")]
		[RegularExpression(@"^[STFG]\d{7}[A-Z]$", ErrorMessage = "Invalid NRIC format.")]
		public string NRIC { get; set; }

		[Required(ErrorMessage = "Please enter a valid date of birth.")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Please upload your resume.")]
		public IFormFile Resume { get; set; }

		[Required(ErrorMessage = "Please provide a brief description.")]
		[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Special characters are allowed")]
		public string WhoAmI { get; set; }
	}
}
