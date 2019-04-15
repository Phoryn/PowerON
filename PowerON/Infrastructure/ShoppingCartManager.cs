using Microsoft.AspNetCore.Http;
using PowerON.DAL;
using PowerON.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerON.Infrastructure
{
    public class ShoppingCartManager
    {
        private readonly StoreContext db;
        public const string CartSessionKey = "CartData";
        private IHttpContextAccessor session;

        public ShoppingCartManager(StoreContext db, IHttpContextAccessor accesor)
        {
            this.db = db;
            this.session = accesor;
        }


        public void AddtoCart(int itemId)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Item.ItemId == itemId);

            if (cartItem != null)
                cartItem.Quantity++;
            else
            {
                var itemToAdd = db.Items.Where(a => a.ItemId == itemId).SingleOrDefault();
                if(itemToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Item = itemToAdd,
                        Quantity = 1,
                        TotalPrice = itemToAdd.Price
                    };

                    cart.Add(newCartItem);
                }
            }
            session.HttpContext.Session.SetObject<List<CartItem>>(CartSessionKey, cart);

        }

        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if(session.HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey);
            }

            return cart;
        }

        public int RemoveFromCart(int itemId)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Item.ItemId == itemId);
            var index = cart.IndexOf(cartItem);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cart[index] = cartItem;
                    session.HttpContext.Session.SetObject<List<CartItem>>(CartSessionKey, cart);
                    return cartItem.Quantity;
                    

                }
                else
                    cart.Remove(cartItem);
            }
            session.HttpContext.Session.SetObject<List<CartItem>>(CartSessionKey, cart);

            return 0;
        }
        
        public decimal GetCartTotalPrice()
        {
            var cart = this.GetCart();
            return cart.Sum(c => (c.Quantity * c.Item.Price));
        }

        public int GetCartItemsCouns()
        {
            var cart = this.GetCart();
            return cart.Sum(c => c.Quantity);
        }
        
        public Order CreateOrder(Order newOrder, string userId)
        {
            var cart = this.GetCart();

            newOrder.DateCreated = DateTime.Now;
            //newOrder.UserId = userId;

            this.db.Orders.Add(newOrder);

            if (newOrder.OrderItems == null)
                newOrder.OrderItems = new List<OrderItem>();

            decimal cartTotal = 0;

            foreach (var cartItem in cart)
            {
                var newOrderItem = new OrderItem()
                {
                    AlbumId = cartItem.Item.ItemId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Item.Price
                };

                cartTotal += (cartItem.Quantity * cartItem.Item.Price);

                newOrder.OrderItems.Add(newOrderItem);
            }

            newOrder.TotalPrice = cartTotal;

            this.db.SaveChanges();

            return newOrder;
        }

        public void EmptyCart()
        {
            session.HttpContext.Session.SetObject<List<CartItem>>(CartSessionKey, null);
        }
    }
}
