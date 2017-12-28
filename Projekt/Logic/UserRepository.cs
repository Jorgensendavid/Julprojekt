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
            usr.UserPhoto = user.UserPhoto;
            db.SaveChanges();

        }
        public List<ApplicationUser> StartUsers()
        {
            List<ApplicationUser> RandomUsers()
            {
                var list = new List<ApplicationUser>();
                var randomUser = db.Users.OrderBy(x => Guid.NewGuid()).ToList();

                list.Add(randomUser[0]);
                //list.Add(randomUser[1]);
                // list.Add(randomUser[2]);

                return list;
            }
            return RandomUsers();
        }

        public bool AlreadyFriends(ApplicationUser user, string id)
        {
            var usr = getUserName(user.UserName);
            var userID = db.Users.Single(u => u.Id == id);
            var AllFriends = db.Friends.ToList();
            foreach (Friend friends in AllFriends)
            {
                if(friends.Requester == user && friends.Receiver == userID)
                { return true; }
                if(friends.Requester == userID && friends.Receiver == user)
                    { return true; }
            }
            return false;
        }
    }
}
