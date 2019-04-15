using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewComponents
{
    public class CartCount : ViewComponent
    {
        private StoreContext _context;
        readonly HttpContextAccessor session = new HttpContextAccessor();

        public CartCount(StoreContext context)
        {
            _context = context;
        }
        public string Invoke(string genre)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(_context, session);

            var count = shoppingCartManager.GetCartItemsCouns();
            return count.ToString();
        }
    }
}
