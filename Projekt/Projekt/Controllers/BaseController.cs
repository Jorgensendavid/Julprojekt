using Logic;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class BaseController : Controller 
    {
        protected ApplicationDbContext db = new ApplicationDbContext();


        public BaseController()
        {
            //Loopar igenom en lista med vänner för att få fram om man har några nya vänner 
            //och skickar de vidare i en viewbag
            List<Friend> FriendRequest = new List<Friend>();
            {
                var allFriends = db.Friends.ToList();

                foreach (Friend friend in allFriends)
                {
                    if(friend.Accepted == false)
                    {
                        FriendRequest.Add(friend);
                    }
                }
            }
            ViewBag.NewFriend = FriendRequest;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }


    }
}