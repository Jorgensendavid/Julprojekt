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

        public ActionResult ProfileInfo()
        {
            var user = userRepository.getUserName(User.Identity.Name);
            ProfileViewModel model = new ProfileViewModel()
            {
                Alias = user.Alias,
                TextAbout = user.TextAbout,


            };
            return View(model);
        }
    }
}