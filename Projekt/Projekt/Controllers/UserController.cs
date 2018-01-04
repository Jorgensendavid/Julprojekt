using Logic;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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

            ApplicationUser applicationUser = new ApplicationUser();

            var userName = User.Identity.Name;
            applicationUser.UserName = userName;
            applicationUser.UserPhoto = imageData;
            applicationUser.Alias = model.NewName;
            applicationUser.TextAbout = model.About;
            applicationUser.Age = model.Age;
            userRepository.edit(applicationUser);
            return RedirectToAction("ProfileInfo");
        }
        [Authorize]
        public ActionResult ProfileInfo(ProfileViewModel model)
        {
            var user = userRepository.getUserName(User.Identity.Name);

            model.Alias = user.Alias;
            model.TextAbout = user.TextAbout;

            var userName = User.Identity.Name;
            var sender1 = db.Users.Single(x => x.UserName == userName);
            var listOfPosts = new PostsController().GetRelevantPosts(sender1.Id);
           
            var postList = new List<string[]>();
            if (listOfPosts.Count > 0)
            {
                foreach (Post post in listOfPosts)
                {
                    ApplicationUser sender = db.Users.Find(post.From.Id);
                    string name = sender.Alias;
                    string[] postArray = new string[2] { name, post.Text };
                    postList.Add(postArray);
                }
            }
            else
            {
                string[] postArray = new string[2] { "You have no Posts yet", "" };
                postList.Add(postArray);
            }

            ViewBag.list = postList;

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

            var userid = db.Users.Single(x => x.Id == id);

            var listOfPosts = new PostsController().GetRelevantPosts(userid.Id);

            var postList = new List<string[]>();
            if (listOfPosts.Count > 0)
            {
                foreach (Post post in listOfPosts)
                {
                    ApplicationUser sender = db.Users.Find(post.From.Id);
                    string name = sender.Alias;
                    string[] textArray = new string[2] { name, post.Text };
                    postList.Add(textArray);
                }
            }
            else
            {
                string[] textArray = new string[2] { "You have no Posts yet", "" };
                postList.Add(textArray);
            }

          
            var userName = User.Identity.Name;
            var sender1 = db.Users.Single(x => x.UserName == userName);
            ViewBag.Sender = sender1.Id;
            ViewBag.list = postList;
            ViewBag.profilID = userid.Id;

            ViewBag.AlreadyFriends = false;
            if (AlreadyFriends(sender1.Id, userid.Id))
            {
                ViewBag.AlreadyFriends = true;
            }

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
            return RedirectToAction("OthersProfileInfo", new { Id = id});

        }

       
        [Authorize]
        public ActionResult ListPotentialFriends()
        {

            List<Friend> NewFriendsList = new List<Friend>();
            {
                var user = User.Identity.Name;
                var userName = db.Users.Single(x => x.UserName == user);

                var AllFriends = db.Friends.ToList();
                foreach (Friend friends in AllFriends)
                {
                    if (friends.Accepted == false && friends.Requester != userName && friends.Receiver == userName)
                    {
                        NewFriendsList.Add(friends);
                    }
                }

            }
          
            return View(new FriendViewModel { Friends = NewFriendsList });
        }
        [Authorize]
        public ActionResult Friends()
        {
            var userName = User.Identity.Name;
            var user = db.Users.Single(x => x.UserName == userName);

            var AllFriends = db.Friends.ToList();
            var AcceptedFriendsList = new List<Friend>();
            foreach (Friend friend in AllFriends)
            {
                if (friend.Accepted == true)
                {
                    AcceptedFriendsList.Add(friend);
                }
            }
            var MyAcceptedFriends = new List<ApplicationUser>();
            var UserList = db.Users.ToList();

            foreach (ApplicationUser appuser in UserList)
            {
                foreach (Friend friend in AcceptedFriendsList)
                {
                    if (( user == friend.Receiver && appuser.Id.Equals(friend.Requester.Id) || user == friend.Requester && appuser.Id.Equals(friend.Receiver.Id)))
                    {
                        MyAcceptedFriends.Add(appuser);
                    }
                }
            }
            return View(MyAcceptedFriends);
        }


        public ActionResult AcceptFriend(string id, Friend friend)
        {

            List<Friend> FriendsWithRequestPendingList = new List<Friend>();
            {
                var allFriends = db.Friends.ToList();
                foreach (Friend friends in allFriends)
                {
                    if (friends.Accepted == false)
                    {
                        FriendsWithRequestPendingList.Add(friends);
                    }
                }

            }
            var userName = User.Identity.Name;

             foreach (Friend notacceptedfriend in FriendsWithRequestPendingList)
            {
                if (notacceptedfriend.ID.ToString() ==id && notacceptedfriend.Receiver.UserName.Equals(userName))
                {
                    notacceptedfriend.Accepted = true;
                }
            }
            db.SaveChanges();
            return RedirectToAction("ListPotentialFriends");
        }
        public bool AlreadyFriends(string id1, string id2)
        {
            var userID = db.Users.Single(u => u.Id == id1);
            var AllFriends = db.Friends.ToList();
            foreach (Friend friends in AllFriends)
            {
                if (friends.Requester.Id == id2 && friends.Receiver == userID)
                { return true; }
                if (friends.Requester == userID && friends.Receiver.Id == id2)
                { return true; }
            }
            return false;
        }
    }
    
}