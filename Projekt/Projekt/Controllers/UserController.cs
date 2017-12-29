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

        //public ActionResult AcceptFriends(string id, Friend friended)
        //{

        //    List<Friend> FriendList = new List<Friend>();
        //    {
        //        var allFriends = db.Friends.ToList();
        //        foreach (Friend friends in allFriends)
        //        {
        //            if(friends.Accepted == false)
        //            {
        //                FriendList.Add(friends);
        //            }
        //        }
        //    }
        //    var user = db.Users.Single(x => x.Id == id);

        //    var listOFFriends = new List<Friend>();
        //    foreach (Friend friends in FriendList)
        //    {
        //        if (friends.Receiver == user)
        //            listOFFriends.Add(friends);
        //    }
        //    var UserList = db.Users.ToList();
        //    var userName = User.Identity.Name;
        //    var username = db.Users.Single(x => x.UserName == userName);
        //    friended.Requester = username;
            

        //    var PotentialFriends = new List<ApplicationUser>();
        //    foreach (ApplicationUser users in UserList)
        //    {
        //        foreach (Friend friend in listOFFriends)
        //        {

        //            if(users.Id.Equals(username))
        //            {
        //                PotentialFriends.Add(users);
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
        [Authorize]
        public ActionResult ListPotentialFriends(string id)
        {
            var userName = User.Identity.Name;
            var username = db.Users.Single(x => x.UserName == userName);
           

            var friends = db.Friends.Where(x => x.Receiver.Id == id).ToList();
          
            return View(new FriendViewModel {Friends = friends });


            
        }
        public ActionResult MyfriendRequests(ApplicationUser model)
        {
            var findUser = from m in db.Users
                             select m;
            return View(findUser);
        }
    }
}