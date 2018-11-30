using ECommerce_Site.Models;
using ECommerceSite.DAL.Services;
using ECommerceSite.DAL.Services.Implementations;
using System.Linq;
using System.Web.Mvc;

namespace ECommerce_Site.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;

        public CartController() : this(new SqlCartService())
        { }

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        // GET: Cart
        public ActionResult Index()
        {
            var id = GetIdForCurrentUser();
            var Cart = _cartService.GetCartForUser(id);
            return View(Cart);
        }

        private string GetIdForCurrentUser()
        {
            string id = null;
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                id = user.Id;
            }
            return id;
        }
    }
}