using PowerON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Item> Bestsellers { get; set; }
        public IEnumerable<Item> NewArrivals { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
