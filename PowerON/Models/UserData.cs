using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Addresss { get; set; }
        public string CodeAndCity { get; set; }
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string PhonrNumber { get; set; }
        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }
    }
}
