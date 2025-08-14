using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Username, please")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Password, please")]
        public string Password { get; set; }
    }
}
