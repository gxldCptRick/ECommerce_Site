using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce_Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is a story about how my life got twist turned upside down and I would like to take a minute just sit right there and tell you a story about how I became the prince of a town called bel-air";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Don't.";

            return View();
        }
    }
}