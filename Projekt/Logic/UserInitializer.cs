using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class UserInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Users.Add( new ApplicationUser { Email = "David@Random.com", PasswordHash = "David1!", Alias = "David" });
            context.Users.Add(new ApplicationUser { Email = "Sture@Stoppmur.com", PasswordHash = "Sture1!", Alias = "Sture" });
            context.Users.Add(new ApplicationUser { Email = "Amanda@Random.com", PasswordHash = "Amanda1!", Alias = "Amanda" });
          
            base.Seed(context);
        }
    }
}
