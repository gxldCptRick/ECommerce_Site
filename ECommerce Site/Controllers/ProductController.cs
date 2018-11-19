using ECommerce_Site.Models;
using ECommerceSite.DAL.Models;
using ECommerceSite.DAL.Services;
using ECommerceSite.DAL.Services.Implementations;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerce_Site.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController() : this(new SqlProductService())
        { }
        public ProductController(IProductService service)
        {
            _service = service;
        }

        public ActionResult Catalog()
        {
            var allDaProducts = _service.GetAllProducts();

            var listView = new ProductList
            {
                Products = allDaProducts.ToList()
            };

            return View(listView);
        }

        public ActionResult Detailed(int? id)
        {
            return ProccessId("Detailed", id);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.IsCreate = true;
            ViewBag.PostUrl = $"/Product/Create";
            return View("ProductEditor");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, string details)
        {
            var result = default(ActionResult);
            if (details is null) throw new ArgumentNullException(nameof(details), "Details must be provided even if it was only an empty string.");
            product.Details = details.Split('|').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            _service.CreateProduct(product);
            result = RedirectToAction(nameof(Catalog), nameof(Product));
            ViewBag.IsCreate = true;
            return result;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Update(int? id)
        {
            ViewBag.IsCreate = false;
            ViewBag.PostUrl = $"/Product/Update";
            return ProccessId("ProductEditor", id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Product product, string details)
        {
            var result = default(ActionResult);
            if (details is null) throw new ArgumentNullException(nameof(details), "Details must be provided even if it was only an empty string.");
            product.Details = details.Split('|').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (ModelState.IsValid)
            {
                _service.UpdateProduct(product);
                result = RedirectToAction(nameof(Detailed), nameof(Product), new { id = product.Id });
            }
            else
            {
                ViewBag.IsCreate = false;
                ViewBag.PostUrl = "/Product/Update";
                result = View("ProductEditor", product);
            }

            return result;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            return ProccessId(nameof(Delete), id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _service.DeleteProduct(new Product { Id = id });
            return RedirectToAction(nameof(Catalog), nameof(Product));
        }

        private ActionResult ProccessId(string viewName, int? id)
        {
            var result = default(ActionResult);
            if (id is null)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var product = _service.GetProductById(id.Value);
                if (product is null)
                {
                    result = HttpNotFound();
                }
                else
                {
                    result = View(viewName, product);
                }
            }
            return result;
        }

    }
}