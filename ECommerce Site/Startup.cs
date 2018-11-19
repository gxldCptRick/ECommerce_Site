using ECommerce_Site.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(ECommerce_Site.Startup))]
namespace ECommerce_Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool    
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser
                {
                    Email = "testing@tester.com",
                    UserName = "testing@tester.com"
                };

                var userPWD = "WhatIsHappening@@@";

                var createdUser = UserManager.Create(user, userPWD);
                
                if (createdUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
     
            if (!roleManager.RoleExists("Basic"))
            {
                var role = new IdentityRole
                {
                    Name = "Basic"
                };
                roleManager.Create(role);

            }
        }
    }
}
