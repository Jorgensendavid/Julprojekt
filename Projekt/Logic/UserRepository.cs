using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
   public class UserRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ApplicationUser getUserName(string userName)
        {
            var user = db.Users.Single(u => u.UserName.Equals(userName));
            return user;
        }

        public void edit(ApplicationUser user)
        {
            var usr = getUserName(user.UserName);

            usr.TextAbout = user.TextAbout;
            usr.Alias = user.Alias;
            db.SaveChanges();

        }
    }
}
