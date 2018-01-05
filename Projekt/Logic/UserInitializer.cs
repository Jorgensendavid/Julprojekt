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
            //ger exempelanvändarna en default bild
            byte[] imageData = null;
            string fileName = HttpContext.Current.Server.MapPath(@"~/Images/noImg.png");

            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            //skapar 19st användare som kan användas som exempelanvändare i projektet
            for (int i = 0; i < 20; i++)
            {
                var user = new ApplicationUser { UserName = $"test{i}@test.se", Email = $"test{i}@test.se", Alias = $"user{i}", UserPhoto = imageData, invisibile = true };
                userManager.CreateAsync(user, "Test123!").Wait();
            }

            base.Seed(context);
        }
    }
}
