using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.Models;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        //private IMailService mailService;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public ManageController(SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager
                                //IMailService mailService,
                                )
        {
            //this.mailService = mailService;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            LinkSuccess,
            Error
        }


        
        /////////////////////////////////
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            //var z = JsonConvert.DeserializeObject(TempData["ViewData"].ToString());


            if (TempData["error"] != null)
            {
                var z = (Array)TempData["error"];
                foreach (var x in z)
                {
                    ViewData.ModelState.AddModelError("password-error", x.ToString());
                }
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var otherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //var otherLoginsproviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //var z = _db.UserData.Where(a => a.UserDataId == user.UserDataId).SingleOrDefault();

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                HasPassword = this.HasPassword(),
                CurrentLogins = userLogins,
                OtherLogins = otherLogins,
                ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1,
                ChangeProfileViewModel = new ChangeProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    CodeAndCity = user.CodeAndCity,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email

                }
            };

            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "ChangeProfileViewModel")]ChangeProfileViewModel changeProfileViewModel)
        {


            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                user.FirstName = changeProfileViewModel.FirstName;
                user.LastName = changeProfileViewModel.LastName;
                user.Address = changeProfileViewModel.Address;
                user.CodeAndCity = changeProfileViewModel.CodeAndCity;
                user.PhoneNumber = changeProfileViewModel.PhoneNumber;
                user.Email = changeProfileViewModel.Email;

                var result = await _userManager.UpdateAsync(user);



                 AddErrors(result);

                

            }

            if (TempData["error"] != null)
            {
                //TempDataExtend.Put(TempData, "ViewData", ViewData);
                //TempData.Keep("error");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            // In case we have simple errors - return
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                //var user1 = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            // In case we have login errors
            if (TempData["error"] != null)
            {
                //TempData.Keep("error");
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword([Bind(Prefix = "SetPasswordViewModel")]SetPasswordViewModel model)
        {
            // In case we have simple errors - return
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }


            var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    //var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);

                if (TempData["error"] != null)
                {
                    //TempData.Keep("error");
                    return RedirectToAction("Index");
                }
            }

            var message = ManageMessageId.SetPasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (result.Succeeded)
            {
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Index", new { Message = message });
        }
        
        public ActionResult LinkLogin(string returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("LinkLoginCallback", "Manage", new { returnUrl = returnUrl }));
            return Challenge(properties, "Facebook");
        }
        /////////////////////////////////



        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));


            var loginInfo = _signInManager.GetExternalLoginInfoAsync().Result;
            if (loginInfo == null)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }

            var result = await _userManager.AddLoginAsync(user, loginInfo);
            return result.Succeeded ? RedirectToAction("Index", new { Message = ManageMessageId.LinkSuccess }) : RedirectToAction("Index", new { Message = ManageMessageId.Error });
        }
        private void AddErrors(IdentityResult result)
        {
            if (result.Succeeded == false)
            {
                List<string> list = new List<string>();

                foreach (var error in result.Errors)
                {

                    list.Add(error.Description);
                }
                TempData["error"] = list;
            }
            //TempData.Keep("error");

        }
        private bool HasPassword()
        {
            var user = _userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user != null)
            {
                return user.Result.PasswordHash != null;
            }
            return false;
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent });
        }




        //public ActionResult OrdersList()
        //{
        //    bool isAdmin = User.IsInRole("Admin");
        //    ViewBag.UserIsAdmin = isAdmin;

        //    IEnumerable<Order> userOrders;

        //    // For admin users - return all orders
        //    if (isAdmin)
        //    {
        //        userOrders = _db.Orders.Include("OrderItems").
        //            OrderByDescending(o => o.DateCreated).ToArray();
        //    }
        //    else
        //    {
        //        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        userOrders = _db.Orders.Where(o => o.UserId == userId).Include("OrderItems").
        //            OrderByDescending(o => o.DateCreated).ToArray();
        //    }

        //    return View(userOrders);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public OrderState ChangeOrderState(Order order)
        //{
        //    Order orderToModify = _db.Orders.Find(order.OrderId);
        //    orderToModify.OrderState = order.OrderState;
        //    _db.SaveChanges();

        //    if (orderToModify.OrderState == OrderState.Shipped)
        //    {
        //        // Schedule confirmation
        //        //string url = Url.Action("SendStatusEmail", "Manage", new { orderid = orderToModify.OrderId, lastname = orderToModify.LastName }, Request.Url.Scheme);

        //        //BackgroundJob.Enqueue(() => Helpers.CallUrl(url));

        //        //IMailService mailService = new HangFirePostalMailService();
        //        //mailService.SendOrderShippedEmail(orderToModify);

        //        //mailService.SendOrderShippedEmail(orderToModify);

        //        //dynamic email = new Postal.Email("OrderShipped");
        //        //email.To = orderToModify.Email;
        //        //email.OrderId = orderToModify.OrderId;
        //        //email.FullAddress = string.Format("{0} {1}, {2}, {3}", orderToModify.FirstName, orderToModify.LastName, orderToModify.Address, orderToModify.CodeAndCity);
        //        //email.Send();
        //    }

        //    return order.OrderState;
        //}

        //[AllowAnonymous]
        //public ActionResult SendStatusEmail(int orderid, string lastname)
        //{
        //    // This could also be used (but problems when hosted on Azure Websites)
        //    // if (Request.IsLocal)            

        //    var orderToModify = _db.Orders.Include("OrderItems").Include("OrderItems.Album").SingleOrDefault(o => o.OrderId == orderid && o.LastNAme == lastname);

        //    if (orderToModify == null) return StatusCode(404);

        //    //OrderShippedEmail email = new OrderShippedEmail();
        //    //email.To = orderToModify.Email;
        //    //email.OrderId = orderToModify.OrderId;
        //    //email.FullAddress = string.Format("{0} {1}, {2}, {3}", orderToModify.FirstName, orderToModify.LastName, orderToModify.Address, orderToModify.CodeAndCity);
        //    //email.Send();

        //    return StatusCode(200);
        //}

        //[AllowAnonymous]
        //public ActionResult SendConfirmationEmail(int orderid, string lastname)
        //{
        //    // orderid and lastname as a basic form of auth

        //    // Also might be called by scheduler (ie. Azure scheduler), pinging endpoint and using some kind of queue / db

        //    // This could also be used (but problems when hosted on Azure Websites)
        //    // if (Request.IsLocal)            

        //    var order = _db.Orders.Include("OrderItems").Include("OrderItems.Album").SingleOrDefault(o => o.OrderId == orderid && o.LastNAme == lastname);

        //    if (order == null) return StatusCode(404);

        //    //OrderConfirmationEmail email = new OrderConfirmationEmail();
        //    //email.To = order.Email;
        //    //email.Cost = order.TotalPrice;
        //    //email.OrderNumber = order.OrderId;
        //    //email.FullAddress = string.Format("{0} {1}, {2}, {3}", order.FirstName, order.LastName, order.Address, order.CodeAndCity);
        //    //email.OrderItems = order.OrderItems;
        //    //email.CoverPath = AppConfig.PhotosFolderRelative;
        //    //email.Send();

        //    return StatusCode(200);
        //}

        //[Authorize(Roles = "Admin")]
        //public ActionResult AddProduct(int? itemId, bool? confirmSuccess)
        //{
        //    if (itemId.HasValue)
        //        ViewBag.EditMode = true;
        //    else
        //        ViewBag.EditMode = false;

        //    var result = new EditProductViewModel();
        //    var genres = _db.Genres.ToArray();
        //    result.Genres = genres;
        //    result.ConfirmSuccess = confirmSuccess;

        //    Item a;

        //    if (!itemId.HasValue)
        //    {
        //        a = new Item();
        //    }
        //    else
        //    {
        //        a = _db.Items.Find(itemId);
        //    }

        //    result.Item = a;

        //    return View(result);
        //}

        //[HttpPost]
        //public ActionResult AddProduct(HttpPostedFileBase file, EditProductViewModel model)
        //{
        //    if (model.Item.ItemId > 0)
        //    {
        //        // Saving existing entry

        //        _db.Entry(model.Item).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return RedirectToAction("AddProduct", new { confirmSuccess = true });
        //    }
        //    else
        //    {
        //        // Creating new entry

        //        var f = Request.Form;
        //        // Verify that the user selected a file
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            // Generate filename

        //            var fileExt = Path.GetExtension(file.FileName);
        //            var filename = Guid.NewGuid() + fileExt;

        //            var path = Path.Combine(IServer.MapPath(AppConfig.PhotosFolderRelative), filename);
        //            file.SaveAs(path);

        //            // Save info to DB
        //            model.Item.ImageFileName = filename;
        //            model.Item.DateAdded = DateTime.Now;

        //            _db.Entry(model.Item).State = EntityState.Added;
        //            _db.SaveChanges();

        //            return RedirectToAction("AddProduct", new { confirmSuccess = true });
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Nie wskazano pliku.");
        //            var genres = _db.Genres.ToArray();
        //            model.Genres = genres;
        //            return View(model);
        //        }
        //    }

        //}

        //public ActionResult HideProduct(int itemId)
        //{
        //    var album = _db.Items.Find(itemId);
        //    album.IsHidden = true;
        //    _db.SaveChanges();

        //    return RedirectToAction("AddProduct", new { confirmSuccess = true });
        //}

        //public ActionResult UnhideProduct(int itemId)
        //{
        //    var album = _db.Items.Find(itemId);
        //    album.IsHidden = false;
        //    _db.SaveChanges();

        //    return RedirectToAction("AddProduct", new { confirmSuccess = true });
        //}


    }
}