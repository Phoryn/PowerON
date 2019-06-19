using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Models;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }




        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, 
                                                            model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl});
                }
                else
                {
                    ModelState.AddModelError("loginerror", "Nieudana próba logowania.");
                    return View(model);
                }
            }
                return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                // Być może do usunięcia jeśli błędy będą obsługiwane przez viewmodel
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError("", error.Description);
                //}
                //
                AddErrors(result);
            }
            return View(model);

        }

        public IActionResult SignIn(string provider, string returnUrl)
        {

            //return Challenge(new AuthenticationProperties { RedirectUri = "account/facebook" }, provider);
            //return new ChallengeResult(provider, new AuthenticationProperties());
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl}));
            return Challenge(properties, "Facebook");

            //return RedirectToAction("Facebook", "Account", new { returnUrl = returnUrl });
        }


        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            //info.Principal //the IPrincipal with the claims from facebook
            //info.ProviderKey //an unique identifier from Facebook for the user that just signed in
            //info.LoginProvider //a string with the external login provider name, in this case Facebook
            
            //to sign the user in if there's a local account associated to the login provider
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            //result.Succeeded; //will be false if there's no local associated account 

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);

            }
            else
            {
                var user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    UserData = new UserData { Email = info.Principal.FindFirstValue(ClaimTypes.Email) }
                };
                var registrationResult = await userManager.CreateAsync(user);
                if (registrationResult.Succeeded)
                {
                    registrationResult = await userManager.AddLoginAsync(user, info);
                    if (registrationResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                        throw new Exception("Duda Błąd! External provider assocation error");
                }
                else
                    throw new Exception("Duda błąd! Registration error");
            }
            

            //to associate a local user account to an external login provider
            //await userManager.AddLoginAsync(aUserYoullHaveToCreate, info);        
            /*return Redirect("~/");*/
        }



        //public async  Task<IActionResult> Facebook(string returnUrl)
        //{
        //    var result = await HttpContext.AuthenticateAsync(TemporaryAuthenticationDefaults.AuthenticationScheme);
        //    if (!result.Succeeded)
        //    {
        //        return RedirectToAction("SignIn");
        //    }

        //    var username = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var username2 = result.Principal.FindFirstValue(ClaimTypes.Name);

            

        //    var result2 = signInManager.ExternalLoginSignInAsync(username2, username, isPersistent: false);


        //    return RedirectToLocal(returnUrl);

        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}