using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerON.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewComponents
{
    public class LoginPartial : ViewComponent
    {
        private StoreContext _context;
        public LoginPartial(StoreContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
    }
}
