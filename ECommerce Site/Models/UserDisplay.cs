using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Site.Models
{
    public class UserDisplay
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}