using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int GenreId { get; set; }
        public string ItemTitle { get; set; }
        public string ItemName { get; set; }
        public DateTime DateAdded { get; set; }
        public string ImageFileName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsHidden { get; set; }

        public virtual Genre Genre { get; set;  }
    }
} 