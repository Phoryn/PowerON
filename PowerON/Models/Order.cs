﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class Order
    {
        //[Key]
        public int OrderId { get; set; }

        //[ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić imię")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzć nazwisko")]
        [StringLength(150)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Nie wprowadzono adresu")]
        [StringLength(150)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Wprowadź kod pocztowy i miasto")]
        [StringLength(50)]
        public string CodeAndCity { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić numer telefonu")]
        [StringLength(20)]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+",
        ErrorMessage = "Błędny format numeru telefonu.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Wprowadź swój adres e-mail.")]
        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }

        public string Comment { get; set; }

        public DateTime DateCreated { get; set; }

        public OrderState OrderState { get; set; }


        [Column(TypeName = "decimal(9,2)")]
        public decimal TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public virtual ApplicationUser User { get; set; }

    }

    public enum OrderState
    {
        [Display(Name = "nowe")]
        New,

        [Display(Name = "wysłane")]
        Shipped
    }
}
