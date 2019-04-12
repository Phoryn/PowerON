using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class CartItem
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
