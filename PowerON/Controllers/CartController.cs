using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    public class CartController : Controller
    {
        private ShoppingCartManager shoppingcartManager;
        private ISession Session { get; set; }
        private readonly StoreContext db;

        public CartController(StoreContext context, IHttpContextAccessor session)
        {
            this.Session = session.HttpContext.Session;
            this.db = context;
            this.shoppingcartManager = new ShoppingCartManager(db, this.Session);
        }

        public IActionResult Index()
        {
            var cartItems = shoppingcartManager.GetCart();
            var cartTotalPrice = shoppingcartManager.GetCartTotalPrice();

            CartViewModel cartVM = new CartViewModel()
            {
                CartItems = cartItems,
                TotalPrice = cartTotalPrice
            };

            return View(cartVM);
        }

        public IActionResult AddToCart(int id)
        {
            shoppingcartManager.AddtoCart(id);

            return RedirectToAction("index");
        }
    }
}