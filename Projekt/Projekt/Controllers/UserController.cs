using Logic;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class UserController : BaseController
    {
        UserRepository userRepository = new UserRepository();
        public ActionResult Index(string searchString,ApplicationUser model)
        {

            var findUser = from m in db.Users
                           where m.invisibile == true
                           select m ;

            
                if (!string.IsNullOrEmpty(searchString))
                {
                    findUser = findUser.Where(s => s.Alias.Contains(searchString));

                }
            
            return View(findUser);
        }
      
        public ActionResult editProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult editProfile([Bind(Exclude = "UserPhoto")]editProfileViewModel model)
        {
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            //Here we pass the byte array to user context to store in db
            ApplicationUser applicationUser = new ApplicationUser();

            var userName = User.Identity.Name;
            applicationUser.UserName = userName;
            applicationUser.UserPhoto = imageData;
            applicationUser.Alias = model.NewName;
            applicationUser.TextAbout = model.About;
            userRepository.edit(applicationUser);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ProfileInfo(ProfileViewModel model)
        {
            var user = userRepository.getUserName(User.Identity.Name);

            model.Alias = user.Alias;
            model.TextAbout = user.TextAbout;
            return View(model);
        }
        
        [Authorize]
        public ActionResult OthersProfileInfo(string id)
        {
            var user = userRepository.getUserId(id);

            ViewProfilesModel watchProfiles = new ViewProfilesModel()
            {
                Alias = user.Alias,
                TextAbout = user.TextAbout,
                ProfileID = id,               
            };
            return View(watchProfiles);
        }

        public ActionResult AddFriends(Friend friend, string id)
        {
            var userName = User.Identity.Name;
            var user = db.Users.Single(x => x.UserName == userName);

            friend.Requester = user;

            var toUser = db.Users.Single(x => x.Id == id);
            friend.Receiver = toUser;
            db.Friends.Add(friend);

            db.SaveChanges();
            return RedirectToAction("Index");

        }

       
        [Authorize]
        public ActionResult ListPotentialFriends(string id)
        {

            List<Friend> NewFriendsList = new List<Friend>();
            {
                var AllFriendConnections = db.Friends.ToList();
                foreach (Friend friendconnection in AllFriendConnections)
                {
                    if (friendconnection.Accepted == false)
                    {
                        NewFriendsList.Add(friendconnection);
                    }
                }

            }
            
            //Skapar USER-objekt av dom aktuella vänförfrågningarna och skickar med till en list-view
         
            //var userName = User.Identity.Name;
            //var username = db.Users.Single(x => x.UserName == userName);


            //var friends = db.Friends.Where(x => x.Receiver.Id == id).ToList();

            //return View(new FriendViewModel {Friends = friends });
            return View(new FriendViewModel { Friends = NewFriendsList });


        }
        [Authorize]
        public ActionResult Friends()
        {

            //hämtar aktuella användaren
            //hämtar alla vänkombinationer
            var userName = User.Identity.Name;
            var user = db.Users.Single(x => x.UserName == userName);

            var AllFriendConnections = db.Friends.ToList();
            //skapar en tom lista för alla accepterade vänner
            var AcceptedFriendsList = new List<Friend>();
            //lägger till alla accepterade i listan
            foreach (Friend friend in AllFriendConnections)
            {
                if (friend.Accepted == true)
                {
                    AcceptedFriendsList.Add(friend);
                }
            }
            //skapar en tom lista för att hålla user-objekt med accepterade vänner
            var MyAcceptedFriendsAsUsers = new List<ApplicationUser>();
            //skapar en lista med alla users
            var UserList = db.Users.ToList();

            foreach (ApplicationUser appuser in UserList)
            {
                //Fyller på listan med den inloggades accepterade vänner.
                foreach (Friend friend in AcceptedFriendsList)
                {
                    if (( user == friend.Receiver && appuser.Id.Equals(friend.Requester.Id) || user == friend.Requester && appuser.Id.Equals(friend.Receiver.Id)))
                    {
                        MyAcceptedFriendsAsUsers.Add(appuser);
                    }
                }
            }
            return View(MyAcceptedFriendsAsUsers);
        }



        public ActionResult MyfriendRequests(ApplicationUser model)
        {
            var userName = User.Identity.Name;
            var findUser = from m in db.Users
                           where m.UserName.Equals(userName)
                             select m;
            return View(findUser);
        }

        public ActionResult AcceptFriend(string id, Friend friend)
        {

            //Hämtar ALLA vänförfrågningar som inte ännu är accepterade
            List<Friend> FriendsWithRequestPendingList = new List<Friend>();
            {
                var AllFriendConnections = db.Friends.ToList();
                foreach (Friend friendconnection in AllFriendConnections)
                {
                    if (friendconnection.Accepted == false)
                    {
                        FriendsWithRequestPendingList.Add(friendconnection);
                    }
                }

            }
            //Hämtar ID på den som skickat vänförfrågan och den som mottagit den.
            var userName = User.Identity.Name;

           


            //Ändrar den aktuella vänförfrågan till Ja (accepterad).
             foreach (Friend notacceptedfriend in FriendsWithRequestPendingList)
            {
                if (notacceptedfriend.ID.ToString() ==id && notacceptedfriend.Receiver.UserName.Equals(userName))
                {
                    notacceptedfriend.Accepted = true;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
    
}