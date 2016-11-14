using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicApp.Migrations;
using MusicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            const string userName = "admin@admin.com";
            const string password = "Admin1-";
            const string roleName = "Admin";
            const string phoneNumber = "0546889421";

            var appUserStore = new UserStore<ApplicationUser>(db);
            var appUserManager = new UserManager<ApplicationUser>(appUserStore);
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(db));

            try
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == userName);
                var role = db.Roles.SingleOrDefault(r => r.Name == roleName);

                if (role == null)
                {
                    appRoleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                    role = db.Roles.SingleOrDefault(r => r.Name == roleName);
                }
                if (user == null)
                {
                    appUserManager.CreateAsync(new ApplicationUser { UserName = userName, PhoneNumber = phoneNumber }, password).Wait();
                    user = db.Users.SingleOrDefault(u => u.UserName == userName);
                }

                var userRole = user.Roles.SingleOrDefault(r => r.RoleId == role.Id);

                if (userRole == null)
                {
                    appUserManager.AddToRoleAsync(user.Id, roleName).Wait();
                }
            }
            catch (Exception ex)
            {
                // Error catched!
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}