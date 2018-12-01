using ECommerceSite.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Site.Models
{
    public class ProductList
    {
        public List<Product> Products { get; set; }
        public string SearchTerm { get; set; }
    }
}