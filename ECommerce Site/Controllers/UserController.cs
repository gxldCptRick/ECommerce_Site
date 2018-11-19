using ECommerce_Site.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerce_Site.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {

        }
        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsAuth = true;
                using (var app = new ApplicationDbContext())
                {
                    var list = new UserList
                    {
                        Users = new List<UserDisplay>()
                    };

                    var things = app.Users.Where(u => u.UserName != "testing@tester.com" && u.UserName != User.Identity.Name).Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        Roles = u.Roles.ToList()
                    }
                    ).ToList();

                    var roles = app.Roles.ToList();

                    foreach (var users in things)
                    {
                        list.Users.Add(new UserDisplay
                        {
                            Id = users.Id,
                            UserName = users.UserName,
                            Role = roles.First(r => r.Id == users.Roles[0].RoleId).Name
                        });
                    }

                    return View(list);
                }
            }
            else
            {
                ViewBag.IsAuth = false;
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string arbritaryString)
        {
            var roles = new Dictionary<string, string>();
            foreach (var userFormId in Request.Form.AllKeys.Where(s => s.Contains("role_")))
            {
                roles[userFormId.Split('_')[1]] = Request.Form[userFormId];
            }
            foreach (var pair in roles)
            {
                var isUserInRole = await UserManager.IsInRoleAsync(pair.Key, pair.Value);
                if(!isUserInRole)
                {
                    var yes = await UserManager.GetRolesAsync(pair.Key);
                    await UserManager.RemoveFromRolesAsync(pair.Key, yes.ToArray());
                    await UserManager.AddToRoleAsync(pair.Key, pair.Value);
                }

            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}