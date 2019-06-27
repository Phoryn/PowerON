using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerON.Models
{
    public class Item
    {

        public int ItemId { get; set; }
        public int GenreId { get; set; }
        public string ItemName { get; set; }
        public DateTime DateAdded { get; set; }
        public string ImageFileName { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsHidden { get; set; }

        public virtual Genre Genre { get; set;  }
    }
} 