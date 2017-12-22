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

        public ApplicationUser getUserId(string id)
        {
            var user = db.Users.Single(u => u.Id == id);
            return user;
        }

        public void edit(ApplicationUser user)
        {
            var usr = getUserName(user.UserName);

            usr.TextAbout = user.TextAbout;
            usr.Alias = user.Alias;
            db.SaveChanges();

        }
        public List<ApplicationUser> StartUsers()
        {
            List<ApplicationUser> RandomUsers()
            {
                var list = new List<ApplicationUser>();
                var randomUser = db.Users.OrderBy(x => Guid.NewGuid()).ToList();

                list.Add(randomUser[0]);
                list.Add(randomUser[1]);
                // list.Add(randomUser[2]);

                return list;
            }
            return RandomUsers();
        }
    }
}
