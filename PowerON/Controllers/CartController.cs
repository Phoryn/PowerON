using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.Models;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly UserManager<ApplicationUser> _usermanager;
        readonly HttpContextAccessor session = new HttpContextAccessor();

        public CartController(StoreContext context,
                              UserManager<ApplicationUser> usermanager)
        {
            this._db = context;
            this._usermanager = usermanager;
        }

        public IActionResult Index()
        {

            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this._db, session);

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
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this._db, session);

            shoppingCartManager.AddtoCart(id);

            return RedirectToAction("index");
        }

        public IActionResult GetCartItemCount()
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this._db, session);

            return Content(shoppingCartManager.GetCartItemsCouns().ToString());
            //ViewBag["Count"] = shoppingcartManager.GetCartItemsCouns();
            //return  shoppingcartManager.GetCartItemsCouns();
        }

        public IActionResult RemoveFromCart(int itemId)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this._db, session);

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
        

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {

            

            if (User.Identity.IsAuthenticated)
            {
                var user = await _usermanager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var order = new Order
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    CodeAndCity = user.CodeAndCity,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Cart") });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order orderdetails)
        {
            

            if (ModelState.IsValid)
            {
                ShoppingCartManager shoppingCartManager = new ShoppingCartManager(this._db, session);
                var user = await _usermanager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var newOrder = shoppingCartManager.CreateOrder(orderdetails, user.Id);

                await TryUpdateModelAsync(user);
                await _usermanager.UpdateAsync(user);
                shoppingCartManager.EmptyCart();

                return RedirectToAction("OrderConfirmation");
            }
            else
            {
                return View(orderdetails);
            }

        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}