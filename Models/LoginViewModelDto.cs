using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;
    public class LoginViewModelDto
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
