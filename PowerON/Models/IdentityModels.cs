using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public virtual ICollection<Order> Orders { get; set; }

        public UserData UserData { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityUser(UserManager<ApplicationUser> manager)
        //{
        //    var userIdentity = await manager.CreateAsync(this, DefaultAuthenticationTypes);

        //    return userIdentity;
        //}
    }
}
