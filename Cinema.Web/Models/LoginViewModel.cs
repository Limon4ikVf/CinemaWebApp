using System.ComponentModel.DataAnnotations;

namespace Cinema.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}