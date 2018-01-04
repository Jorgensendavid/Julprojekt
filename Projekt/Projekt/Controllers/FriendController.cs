using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Projekt.Controllers
{
    public class FriendController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Friend> GetFriendRequest(string id)
        {
            List<Friend> Friends = db.Friends.Where(x => x.Receiver.Id == id && x.Requester.Id!=id && x.Accepted == false).ToList();

            return Friends;
        }
    }
}