using System;
using System.Collections.Generic;
using System.Linq;
using LookaukwatApi.Models;
using LookaukwatApi.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LookaukwatApi.Startup))]

namespace LookaukwatApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //PopulateRole();
        }


        private void PopulateRole()
        {
            var db = new ApplicationDbContext();
            if (!db.Roles.Any(x => x.Name == MyRoleConstant.RoleAdmin))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyRoleConstant.RoleAdmin));
                db.SaveChanges();
            }

            if (!db.Roles.Any(x => x.Name == MyRoleConstant.RoleAgent))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyRoleConstant.RoleAgent));
                db.SaveChanges();

            }

            if (!db.Roles.Any(x => x.Name == MyRoleConstant.Role_Old_Agent))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyRoleConstant.Role_Old_Agent));
                db.SaveChanges();

            }

            if (!db.Roles.Any(x => x.Name == MyRoleConstant.RoleProvider))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyRoleConstant.RoleProvider));
                db.SaveChanges();

            }
            //var user = db.Users.First(s => s.Email == "wan@gmail.com");

            //var Roles = db.Roles.FirstOrDefault(s => s.Name == MyRoleConstant.RoleAdmin);

            //var userRoles = Roles.Users.FirstOrDefault(s => s.UserId == user.Id);

            //if (userRoles != null)
            //    Roles.Users.Remove(userRoles);



            var userStore = new UserStore<ApplicationUser>(db);
            var usermanager = new ApplicationUserManager(userStore);

            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var user = db.Users.First(s => s.Email == "contact@lookaukwat.com");
            var user1 = db.Users.First(s => s.Email == "j@g.com");
            var user2 = db.Users.First(s => s.Email == "h@g.com");
            var user3 = db.Users.First(s => s.Email == "g@gmail.com");
            var user4 = db.Users.First(s => s.Email == "det@gmail.com");
            //var user5 = db.Users.First(s => s.Email == "larissazomo446@gmail.com");
            //var user6 = db.Users.First(s => s.Email == "nembotromeo@gmail.com");
            //var user7 = db.Users.First(s => s.Email == "ebanga.yannick@yahoo.fr");
            //var user8 = db.Users.First(s => s.Email == "bkstyle0905@gmail.com");
            //var user9 = db.Users.First(s => s.Email == "contact@lookaukwat.com");
            //var user10 = db.Users.First(s => s.Email == "wangueujunior23@gmail.com");
            //var user11 = db.Users.First(s => s.Email == "danielnoukeu05@gmail.com");

            //usermanager.AddToRole(user.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user1.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user2.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user3.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user4.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user5.Id, MyRoleConstant.Role_Old_Agent);
            //usermanager.AddToRole(user6.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user7.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user8.Id, MyRoleConstant.RoleAgent);
            //usermanager.AddToRole(user9.Id, MyRoleConstant.RoleAgent);
            usermanager.AddToRole(user.Id, MyRoleConstant.RoleAdmin);
            usermanager.AddToRole(user.Id, MyRoleConstant.RoleAgent);
            usermanager.AddToRole(user1.Id, MyRoleConstant.RoleAgent);
            usermanager.AddToRole(user2.Id, MyRoleConstant.RoleAgent);
            usermanager.AddToRole(user3.Id, MyRoleConstant.RoleAgent);
            usermanager.AddToRole(user4.Id, MyRoleConstant.RoleProvider);
            db.SaveChanges();

        }


    }
}
