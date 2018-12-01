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
            ViewBag.Error = this.Request.QueryString["Error"];
            return View(Cart);
        }

        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            var userId = GetIdForCurrentUser();
            _cartService.RemoveItemFromCart(userId, id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateItem(int id, int amount)
        {
            var userId = GetIdForCurrentUser();
            _cartService.UpdateItemInCart(userId, id, amount);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PurchaseScreen(string id)
        {
            var originalCart = _cartService.GetCartForUser(id);
            if (originalCart.Cart.Count != 0)
            {
                _cartService.CheckoutCart(id);
                return View(originalCart);
            }
            else
            {
                return RedirectToAction("Index", new { Error = "Cart Must Have Items To Checkout" });
            }
        }

        public ActionResult ConfirmScreen(int productId, int amount)
        {
            var id = GetIdForCurrentUser();
            _cartService.AddSpecificAmountOfItemsToCart(id, productId, amount);
            var detail = _cartService.GetCartForUser(id).Cart.First(i => i.ProductId == productId);
            return View(detail);
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