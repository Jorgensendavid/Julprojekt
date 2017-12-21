using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index(string searchString)
        {
            
            var findUser = from m in db.Users
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                findUser = findUser.Where(s => s.Email.Contains(searchString));
            }
            return View(findUser);
        }
      
    }
}