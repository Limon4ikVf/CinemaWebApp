using System.ComponentModel.DataAnnotations;

namespace Cinema.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введіть прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}