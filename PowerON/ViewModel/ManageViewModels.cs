using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PowerON.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewModel
{

    public class ManageCredentialsViewModel
    {
        public bool HasPassword { get; set; }
        public SetPasswordViewModel SetPasswordViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public PowerON.Controllers.ManageController.ManageMessageId? Message { get; set; }
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationScheme> OtherLogins { get; set; }
        public bool ShowRemoveButton { get; set; }
        public ChangeProfileViewModel ChangeProfileViewModel { get; set; }

    }

    public class ChangeProfileViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string CodeAndCity { get; set; }

        [RegularExpression(@"(\+\d{2})*[\d\s-]+",
            ErrorMessage = "Błędny format numeru telefonu.")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie pasują do siebie!")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie pasują do siebie!")]
        public string ConfirmPassword { get; set; }
    }

    public class EditProductViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public bool? ConfirmSuccess { get; set; }
    }
}
