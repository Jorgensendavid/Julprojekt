using Projekt;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Framework;
using System.Web;
using System.IO;

namespace Logic
{
    public class UserInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(store);

            byte[] imageData = null;
            string fileName = HttpContext.Current.Server.MapPath(@"~/Images/noImg.png");

            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);

            for (int i = 0; i < 20; i++)
            {
                var user = new ApplicationUser { UserName = $"test{i}@test.se", Email = $"test{i}@test.se", Alias = $"user{i}", UserPhoto = imageData, invisibile = true };
                userManager.CreateAsync(user, "Test123!").Wait();
            }

            base.Seed(context);
        }

        //private static void SeedUsers(ApplicationDbContext context)
        //{
        //    //var store = new CustomUserStore(context);

        //    //var userManager = new ApplicationUserManager(store);

        //    //var user1 = new ApplicationUser { UserName = "david@david.se", Email = "david@david.se", Alias ="David"  };
        //    //var user2 = new ApplicationUser { UserName = "amanda@amanda.se", Email = "amanda@amanda.se", Alias = "Amanda" };
        //    //var user3 = new ApplicationUser { UserName = "sture@sture.se", Email = "sture@sture.se", Alias = "Sture" };
        //    //var user4 = new ApplicationUser { UserName = "ezequel@ezequel.se", Email = "ezequel@sture.se", Alias = "Ezequel" };

        //    //userManager.CreateAsync(user4, "Ezequel!").Wait();
        //    //userManager.CreateAsync(user3, "Sture!").Wait();
        //    //userManager.CreateAsync(user2, "Amanda1!").Wait();
        //    //userManager.CreateAsync(user1, "David1!").Wait();
        //}
    }
}
