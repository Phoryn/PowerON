using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wprowadź adres e-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wprowadź hasło")]
        public string Password { get; set; }
        [Display(Name = "Pamiętaj mnie!")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100,ErrorMessage = "{0} musi być dłuższe od {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź Hasło")]
        [Compare("Password", ErrorMessage = "Wpisane hasła nie pasują do siebie!" )]
        public bool ConfirmPassword { get; set; }
    }
}
