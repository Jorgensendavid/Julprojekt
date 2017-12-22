using Logic;
using Projekt.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult editProfile(editProfileViewModel model)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            var userName = User.Identity.Name;
            applicationUser.UserName = userName;

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
                ProfileID = id
               
            };
            return View(watchProfiles);
        }
    }
}