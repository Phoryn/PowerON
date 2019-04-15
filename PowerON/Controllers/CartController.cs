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
        private readonly StoreContext db;
        readonly HttpContextAccessor session = new HttpContextAccessor();

        public CartController(StoreContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {

            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this.db, session);

            var cartItems = shoppingCartManager.GetCart();
            var cartTotalPrice = shoppingCartManager.GetCartTotalPrice();

            CartViewModel cartVM = new CartViewModel()
            {
                CartItems = cartItems,
                TotalPrice = cartTotalPrice
            };

            return View(cartVM);
        }

        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this.db, session);

            shoppingCartManager.AddtoCart(id);

            return RedirectToAction("index");
        }

        public IActionResult GetCartItemCount()
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this.db, session);

            return Content(shoppingCartManager.GetCartItemsCouns().ToString());
            //ViewBag["Count"] = shoppingcartManager.GetCartItemsCouns();
            //return  shoppingcartManager.GetCartItemsCouns();
        }

        public IActionResult RemoveFromCart(int itemId)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this.db, session);

            int itemCount = shoppingCartManager.RemoveFromCart(itemId);
            int cartItemsCount = shoppingCartManager.GetCartItemsCouns();
            decimal cartTotal = shoppingCartManager.GetCartTotalPrice();

            var result = new CartRemoveViewModel
            {
                RemoveItemId = itemId,
                RemovedItemCount = itemCount,
                CartTotal = cartTotal,
                CartItemsCount = cartItemsCount
            };

            return Json(result);
        }
    }
}